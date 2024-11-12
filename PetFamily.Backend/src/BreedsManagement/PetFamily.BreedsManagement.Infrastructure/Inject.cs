using Microsoft.Extensions.DependencyInjection;
using PetFamily.BreedsManagement.Application.Abstractions;
using PetFamily.BreedsManagement.Infrastructure.DbContexts;
using PetFamily.BreedsManagement.Infrastructure.Repositories;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddBreedsManagementInfrastructure(this IServiceCollection services)
    {
        services
            .AddDatabase()
            .AddRepositories();
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}