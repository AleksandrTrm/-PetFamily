using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();

        services.AddScoped<IVolunteersRepository, VolunteersRepository>();

        return services;
    }
}