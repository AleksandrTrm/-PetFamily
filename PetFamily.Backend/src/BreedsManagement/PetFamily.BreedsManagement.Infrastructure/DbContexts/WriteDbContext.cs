using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Domain.AggregateRoot;

namespace PetFamily.BreedsManagement.Infrastructure.DbContexts;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Species> Species => Set<Species>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(InfrastructureConstants.DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    } 

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}