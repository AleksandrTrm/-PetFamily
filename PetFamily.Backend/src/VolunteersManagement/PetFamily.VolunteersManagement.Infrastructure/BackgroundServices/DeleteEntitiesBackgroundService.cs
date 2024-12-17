using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetFamily.VolunteersManagement.Infrastructure.BackgroundServices.Options;
using PetFamily.VolunteersManagement.Infrastructure.Services;

namespace PetFamily.VolunteersManagement.Infrastructure.BackgroundServices;

public class DeleteEntitiesBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<DeleteEntitiesBackgroundServiceOptions> options,
    ILogger<DeleteEntitiesBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Delete entities background service is started");

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            var service = scope.ServiceProvider.GetRequiredService<DeleteEntitiesService>();

            logger.LogInformation("Deleted entities service is working");
            
            await service.Process(options.Value.DeletedEntityDaysLifetime, stoppingToken);

            await Task.Delay(TimeSpan.FromHours(options.Value.DeletingEntitiesHoursDelay), stoppingToken);
        }   
    }
}