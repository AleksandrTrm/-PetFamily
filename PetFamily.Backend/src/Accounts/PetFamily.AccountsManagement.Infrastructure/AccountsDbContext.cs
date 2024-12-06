using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.SharedKernel;
using PetFamily.AccountsManagement.Domain.Entities;

namespace PetFamily.AccountsManagement.Infrastructure;

public class AccountsDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE));
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        builder.Entity<User>().ToTable("users");

        builder.Entity<Role>().ToTable("roles");
        
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_token");
        
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_role");

        builder.HasDefaultSchema("accounts");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}