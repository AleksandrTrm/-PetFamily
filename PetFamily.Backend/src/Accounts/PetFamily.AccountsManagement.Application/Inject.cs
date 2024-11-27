﻿using Microsoft.Extensions.DependencyInjection;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.AccountsManagement.Application;

public static class Inject
{
    public static IServiceCollection AddAccountsManagementApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries();

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