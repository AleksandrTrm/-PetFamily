using Microsoft.Extensions.DependencyInjection;
using PetFamily.VolunteersManagement.Contracts;

namespace PetFamily.VolunteersManagement.Presentation;

public static class Inject
{
    public static IServiceCollection AddVolunteersManagementPresentation(this IServiceCollection services)
    {
        services.AddContracts();

        return services;
    }

    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersManagementContracts, VolunteersManagementContracts>();

        return services;
    }
}