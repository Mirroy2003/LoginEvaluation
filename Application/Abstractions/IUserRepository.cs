using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Domain.Entities;

namespace LoginEvaluation.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> FindByDniAsync(string dni, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<bool> DniExistsAsync(string dni, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
