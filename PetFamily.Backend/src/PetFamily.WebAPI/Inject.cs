using PetFamily.AccountsManagement.Application;
using PetFamily.AccountsManagement.Infrastructure;
using PetFamily.BreedsManagement.Application;
using PetFamily.BreedsManagement.Infrastructure;
using PetFamily.BreedsManagement.Presentation;
using PetFamily.VolunteersManagement.Application;
using PetFamily.VolunteersManagement.Infrastructure;
using PetFamily.VolunteersManagement.Presentation;

namespace PetFamily.WebAPI;

public static class Inject
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddVolunteersManagement(configuration);
        services.AddBreedsManagement();
        services.AddAccountManagement(configuration);
        
        return services;
    }

    private static IServiceCollection AddBreedsManagement(this IServiceCollection services)
    {
        services
            .AddBreedsManagementInfrastructure()
            .AddBreedsManagementApplication()
            .AddBreedsManagementPresentation();

        return services;
    }

    private static IServiceCollection AddVolunteersManagement(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddVolunteersManagementInfrastructure(configuration)
            .AddVolunteersManagementApplication()
            .AddVolunteersManagementPresentation();

        return services;
    }

    private static IServiceCollection AddAccountManagement(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAccountsManagementInfrastructure(configuration)
            .AddAccountsManagementApplication();

        return services;
    }
}