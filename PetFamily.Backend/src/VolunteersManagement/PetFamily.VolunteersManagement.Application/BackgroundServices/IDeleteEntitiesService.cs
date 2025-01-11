namespace PetFamily.VolunteersManagement.Application.BackgroundServices;

public interface IDeleteEntitiesService
{
    Task Process(double deletedEntityLifeTime, CancellationToken cancellationToken);
}