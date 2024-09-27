using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Features.Commands.Volunteers.Create;
using PetFamily.Application.Features.Commands.Volunteers.Delete;
using PetFamily.Application.Features.Commands.Volunteers.Pet.AddPet;
using PetFamily.Application.Features.Commands.Volunteers.Pet.UploadPetFiles;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateRequisites;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateSocialMedias;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddQueries()
            .AddCommands()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<>), typeof(ICommandHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes.AssignableToAny(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}