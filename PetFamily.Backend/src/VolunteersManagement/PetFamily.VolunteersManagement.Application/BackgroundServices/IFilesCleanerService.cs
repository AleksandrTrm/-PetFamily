namespace PetFamily.VolunteersManagement.Application.BackgroundServices;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}