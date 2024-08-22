using PetFamily.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetFamily.Domain.Entities.SpeciesAggregate.Species;
using PetFamily.Domain.Entities.Volunteers;
using PetFamily.Domain.Entities.Volunteers.Volunteer;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    private const string DATABASE = "Database";

    public DbSet<Volunteer> Volunteers => Set<Volunteer>();

    public DbSet<Species> Species => Set<Species>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}