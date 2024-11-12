using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.BackgroundServices;
using PetFamily.Shared.Core.Messaging.MessageQueues;
using PetFamily.Shared.Core.Options;
using PetFamily.Shared.Framework.BackgroundServices;
using PetFamily.Shared.SharedKernel.Abstractions;
using PetFamily.VolunteersManagement.Application.Abstractions;
using PetFamily.VolunteersManagement.Infrastructure.DbContexts;
using PetFamily.VolunteersManagement.Infrastructure.Providers;
using PetFamily.VolunteersManagement.Infrastructure.Repositories;

namespace PetFamily.VolunteersManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddVolunteersManagementInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddHostedServices()
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
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddScoped<IFilesCleanerService, FilesCleanerService>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

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