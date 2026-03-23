using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Application.Abstractions;
using LoginEvaluation.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoginEvaluation.Infrastructure.Notifications;

public class SmtpNotificationService : INotificationService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<SmtpNotificationService> _logger;

    public SmtpNotificationService(IOptions<EmailSettings> options, ILogger<SmtpNotificationService> logger)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task NotifyAccountLockedAsync(User user, DateTime lockoutEndUtc, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            _logger.LogWarning("Cannot send lockout email: user {UserId} has no email.", user.Id);
            return;
        }

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpPassword)
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = "Cuenta bloqueada temporalmente",
            Body = string.Join(Environment.NewLine,
                "Hola,",
                "Tu cuenta ha sido bloqueada temporalmente después de 5 intentos fallidos de inicio de sesión.",
                "El bloqueo durará 15 minutos.",
                "Si no reconoces esta actividad, cambia tu contraseña o revisa la seguridad de tu cuenta.",
                "Este es un mensaje automático del sistema."),
            IsBodyHtml = false
        };
        mail.To.Add(new MailAddress(user.Email));

        try
        {
            await client.SendMailAsync(mail, cancellationToken);
            _logger.LogInformation("Lockout email sent to {Email}", user.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send lockout email to {Email}", user.Email);
        }
    }
}

