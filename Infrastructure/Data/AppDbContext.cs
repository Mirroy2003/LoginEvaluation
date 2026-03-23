using LoginEvaluation.Domain.Entities;
using LoginEvaluation.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LoginEvaluation.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}

