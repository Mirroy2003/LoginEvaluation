using System.Threading;
using System.Threading.Tasks;
using LoginEvaluation.Application.Abstractions;
using LoginEvaluation.Domain.Entities;
using LoginEvaluation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoginEvaluation.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> FindByDniAsync(string dni, CancellationToken cancellationToken)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Dni == dni, cancellationToken);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        return _context.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public Task<bool> DniExistsAsync(string dni, CancellationToken cancellationToken)
    {
        return _context.Users.AnyAsync(u => u.Dni == dni, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
