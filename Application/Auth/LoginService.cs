using System;
using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Application.Abstractions;
using LoginEvaluation.Domain.Entities;

namespace LoginEvaluation.Application.Auth;

public class LoginService : IAuthService
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly INotificationService _notificationService;

    public LoginService(IUserRepository users, IPasswordHasher passwordHasher, INotificationService notificationService)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public async Task<AuthResult> AuthenticateAsync(string dni, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(password))
        {
            return AuthResult.Fail();
        }

        var user = await _users.FindByDniAsync(dni.Trim(), cancellationToken);
        if (user is null)
        {
            return AuthResult.Fail();
        }

        if (!user.IsActive)
        {
            return AuthResult.FailInactive();
        }

        if (user.IsLockedOut())
        {
            return AuthResult.LockedOut(user.LockoutEndUtc);
        }

        var passwordIsValid = _passwordHasher.Verify(password, user.PasswordHash);
        if (!passwordIsValid)
        {
            user.RegisterFailedLogin(MaxFailedAttempts, LockoutDuration);
            await _users.SaveChangesAsync(cancellationToken);

            if (user.IsLockedOut())
            {
                await _notificationService.NotifyAccountLockedAsync(user, user.LockoutEndUtc!.Value, cancellationToken);
                return AuthResult.LockedOut(user.LockoutEndUtc);
            }

            return AuthResult.Fail();
        }

        user.MarkLoginSuccess();
        await _users.SaveChangesAsync(cancellationToken);
        return AuthResult.Ok(user);
    }

    public async Task<User> RegisterAsync(string dni, string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dni))
        {
            throw new ArgumentException("DNI is required", nameof(dni));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required", nameof(password));
        }

        var normalizedDni = dni.Trim();
        var normalizedEmail = email.Trim();
        if (await _users.DniExistsAsync(normalizedDni, cancellationToken))
        {
            throw new InvalidOperationException("DNI already registered.");
        }

        if (await _users.EmailExistsAsync(normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Email already registered.");
        }

        var passwordHash = _passwordHasher.Hash(password);
        var user = new User(normalizedDni, normalizedEmail, passwordHash);
        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);
        return user;
    }
}
