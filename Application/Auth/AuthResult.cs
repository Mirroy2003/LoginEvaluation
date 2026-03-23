using LoginEvaluation.Domain.Entities;

namespace LoginEvaluation.Application.Auth;

public sealed class AuthResult
{
    public bool Success { get; init; }
    public bool Inactive { get; init; }
    public bool Locked { get; init; }
    public DateTime? LockoutEndUtc { get; init; }
    public User? User { get; init; }
    public static AuthResult FailInactive() => new() { Success = false, Inactive = true };
    public static AuthResult Fail() => new() { Success = false, Inactive = false };
    public static AuthResult Ok(User user) => new() { Success = true, User = user };
    public static AuthResult LockedOut(DateTime? until) => new() { Success = false, Locked = true, LockoutEndUtc = until };
}
