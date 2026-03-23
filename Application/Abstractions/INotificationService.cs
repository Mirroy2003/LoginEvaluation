using System;
using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Domain.Entities;

namespace LoginEvaluation.Application.Abstractions;

public interface INotificationService
{
    Task NotifyAccountLockedAsync(User user, DateTime lockoutEndUtc, CancellationToken cancellationToken = default);
}

