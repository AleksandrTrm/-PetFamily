namespace PetFamily.Shared.SharedKernel.Abstractions;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken);
}