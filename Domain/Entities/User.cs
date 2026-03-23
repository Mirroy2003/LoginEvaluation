using System;

namespace LoginEvaluation.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Dni { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockoutEndUtc { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public User(string dni, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(dni))
        {
            throw new ArgumentException("DNI is required", nameof(dni));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required", nameof(passwordHash));
        }

        Id = Guid.NewGuid();
        Dni = dni.Trim();
        Email = email.Trim();
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;

    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required", nameof(passwordHash));
        }

        PasswordHash = passwordHash;
    }

    public void RegisterFailedLogin(int maxAttempts, TimeSpan lockoutDuration)
    {
        FailedLoginAttempts++;
        if (FailedLoginAttempts >= maxAttempts)
        {
            LockoutEndUtc = DateTime.UtcNow.Add(lockoutDuration);
        }
    }

    public bool IsLockedOut() => LockoutEndUtc.HasValue && LockoutEndUtc.Value > DateTime.UtcNow;

    public void ResetLockout()
    {
        FailedLoginAttempts = 0;
        LockoutEndUtc = null;
    }

    public void MarkLoginSuccess()
    {
        ResetLockout();
        LastLoginAt = DateTime.UtcNow;
    }
}
