using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetFamily.BreedsManagement.Application.Abstractions;
using PetFamily.Shared.Core.DTOs;

namespace PetFamily.BreedsManagement.Infrastructure.DbContexts;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
    
    public IQueryable<BreedDto> Breeds => Set<BreedDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(InfrastructureConstants.DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    } 

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}