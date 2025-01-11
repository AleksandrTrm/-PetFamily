using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Shared.Core;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Options;
using PetFamily.VolunteersManagement.Application.Abstractions;
using PetFamily.VolunteersManagement.Application.BackgroundServices;
using PetFamily.VolunteersManagement.Application.FileProvider;
using PetFamily.VolunteersManagement.Application.Messaging;
using PetFamily.VolunteersManagement.Infrastructure.BackgroundServices;
using PetFamily.VolunteersManagement.Infrastructure.BackgroundServices.Options;
using PetFamily.VolunteersManagement.Infrastructure.DbContexts;
using PetFamily.VolunteersManagement.Infrastructure.MessageQueues;
using PetFamily.VolunteersManagement.Infrastructure.Providers;
using PetFamily.VolunteersManagement.Infrastructure.Repositories;
using PetFamily.VolunteersManagement.Infrastructure.Services;
using FileInfo = PetFamily.VolunteersManagement.Application.FileProvider.FileInfo;

namespace PetFamily.VolunteersManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddVolunteersManagementInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddHostedServices(configuration)
            .AddDatabase()
            .AddRepositories()
            .AddMinio(configuration);
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();

        return services;
    }
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddScoped<IFilesCleanerService, FilesCleanerService>();
        
        services.Configure<DeleteEntitiesBackgroundServiceOptions>(
            configuration.GetSection(DeleteEntitiesBackgroundServiceOptions
                .DELETE_ENTITIES_BACKGROUND_SERVICE_OPTIONS));
        
        services.AddHostedService<DeleteEntitiesBackgroundService>();

        services.AddScoped<DeleteEntitiesService>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Volunteers);

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}