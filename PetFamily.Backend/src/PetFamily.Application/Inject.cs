﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Pet.AddPet;
using PetFamily.Application.Volunteers.Pet.UploadPetFiles;
using PetFamily.Application.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Update.UpdateRequisites;
using PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddVolunteerHandlers();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }

    private static IServiceCollection AddVolunteerHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<UpdateSocialMediasHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<UploadPetFilesHandler>();

        return services;
    }
}