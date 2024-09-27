using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private IServiceScopeFactory _scopeFactory;
    private ILogger<FilesCleanerBackgroundService> _logger;

    public FilesCleanerBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<FilesCleanerBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Service started");

        await using var scope = _scopeFactory.CreateAsyncScope();

        var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();
        
        while (!stoppingToken.IsCancellationRequested)
            await filesCleanerService.Process(stoppingToken);

        await Task.CompletedTask;
    }
}