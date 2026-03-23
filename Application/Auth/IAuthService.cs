using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Domain.Entities;

namespace LoginEvaluation.Application.Auth;

public interface IAuthService
{
    Task<AuthResult> AuthenticateAsync(string dni, string password, CancellationToken cancellationToken = default);
    Task<User> RegisterAsync(string dni, string email, string password, CancellationToken cancellationToken = default);
}
