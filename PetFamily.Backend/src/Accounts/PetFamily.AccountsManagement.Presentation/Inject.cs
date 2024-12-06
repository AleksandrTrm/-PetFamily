using Microsoft.Extensions.DependencyInjection;
using PetFamily.AccountsManagement.Contracts;

namespace PetFamily.AccountsManagement.Presentation;

public static class Inject
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountsContract, AccountsContract>();

        return services;
    }
}