namespace PetFamily.Infrastructure.BackgroundServices;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}