using Data.Configuration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AuthDbContext : DbContext
{
    public DbSet<UserAccount> UserAccounts { get; set; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
    }
}