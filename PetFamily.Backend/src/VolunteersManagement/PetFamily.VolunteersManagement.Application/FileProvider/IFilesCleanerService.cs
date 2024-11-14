namespace PetFamily.VolunteersManagement.Application.FileProvider;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}