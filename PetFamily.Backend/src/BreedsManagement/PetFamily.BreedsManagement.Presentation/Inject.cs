using Microsoft.Extensions.DependencyInjection;
using PetFamily.BreedsManagement.Contracts;

namespace PetFamily.BreedsManagement.Presentation;

public static class Inject
{
    public static IServiceCollection AddBreedsManagementPresentation(this IServiceCollection services)
    {
        services.AddContracts();

        return services;
    }

    public static IServiceCollection AddContracts(this IServiceCollection services)
    {
        services.AddScoped<IBreedsManagementContracts, BreedsManagementContracts>();

        return services;
    }
}