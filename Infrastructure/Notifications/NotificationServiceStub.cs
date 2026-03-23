using System;
using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Application.Abstractions;
using LoginEvaluation.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace LoginEvaluation.Infrastructure.Notifications;

public class NotificationServiceStub : INotificationService
{
    private readonly ILogger<NotificationServiceStub> _logger;

    public NotificationServiceStub(ILogger<NotificationServiceStub> logger)
    {
        _logger = logger;
    }

    public Task NotifyAccountLockedAsync(User user, DateTime lockoutEndUtc, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Account locked for {Email} until {LockoutEndUtc}", user.Email, lockoutEndUtc);
        return Task.CompletedTask;
    }
}

