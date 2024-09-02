using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Update.UpdateRequisites;
using PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<UpdateSocialMediasHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}