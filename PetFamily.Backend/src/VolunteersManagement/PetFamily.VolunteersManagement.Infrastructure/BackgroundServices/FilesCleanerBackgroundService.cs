using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteersManagement.Application.BackgroundServices;
using PetFamily.VolunteersManagement.Application.FileProvider;

namespace PetFamily.VolunteersManagement.Infrastructure.BackgroundServices;

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
        _logger.LogInformation("Files cleaner service is started");

        await using var scope = _scopeFactory.CreateAsyncScope();

        var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();
        
        while (!stoppingToken.IsCancellationRequested)
            await filesCleanerService.Process(stoppingToken);

        await Task.CompletedTask;
    }
}