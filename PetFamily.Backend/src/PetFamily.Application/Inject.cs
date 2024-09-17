using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Files.Delete;
using PetFamily.Application.Volunteers.Files.Get.GetFile;
using PetFamily.Application.Volunteers.Files.Get.GetFiles;
using PetFamily.Application.Volunteers.Files.Upload;
using PetFamily.Application.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Update.UpdateRequisites;
using PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddVolunteerHandlers();
        services.AddFileHandlers();

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

        return services;
    }
    
    private static IServiceCollection AddFileHandlers(this IServiceCollection services)
    {
        services.AddScoped<UploadFileHandler>();
        services.AddScoped<RemoveFileHandler>();
        services.AddScoped<GetFileHandler>();
        services.AddScoped<GetFilesHandler>();

        return services;
    }
}