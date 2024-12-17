using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteersManagement.Application.BackgroundServices;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;
using PetFamily.VolunteersManagement.Infrastructure.DbContexts;

namespace PetFamily.VolunteersManagement.Infrastructure.Services;

public class DeleteEntitiesService(
    ILogger<DeleteEntitiesService> logger,
    WriteDbContext dbContext) : IDeleteEntitiesService
{
    public async Task Process(double deletedEntityLifetime, CancellationToken cancellationToken)
    {
        var removedVolunteerCount = await DeleteExpiredVolunteers(deletedEntityLifetime, cancellationToken);
        var removedPetsCount = await DeleteExpiredPets(deletedEntityLifetime, cancellationToken);

        logger.LogInformation($"{removedVolunteerCount} expired volunteers has been deleted");
        logger.LogInformation($"{removedPetsCount} expired pets has been deleted");
    }

    private async Task<int> DeleteExpiredPets(
        double deletedEntityLifetime, CancellationToken cancellationToken)
    {
        var totalRemovedPetsCount = 0;

        var volunteersWithDeletedPets = await GetVolunteerWithDeletedPets(cancellationToken);
        foreach (var volunteer in volunteersWithDeletedPets)
            totalRemovedPetsCount += volunteer.IrreversiblePetsDelete(deletedEntityLifetime);

        await dbContext.SaveChangesAsync(cancellationToken);

        return totalRemovedPetsCount;
    }

    private async Task<int> DeleteExpiredVolunteers(double deletedEntityLifetime, CancellationToken cancellationToken)
    {
        var totalDeletedVolunteerCount = 0;

        var deletedVolunteers = await GetDeletedVolunteers(deletedEntityLifetime, cancellationToken);
        foreach (var deletedVolunteer in deletedVolunteers)
        {
            totalDeletedVolunteerCount++;


            deletedVolunteer.DeleteAllPets();
            dbContext.Volunteers.Remove(deletedVolunteer);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return totalDeletedVolunteerCount;
    }

    private async Task<List<Volunteer>> GetDeletedVolunteers(
        double deletedEntityLifetime, CancellationToken cancellationToken)
    {
        return await dbContext.Volunteers
            .Include(v => v.Pets)
            .Where(p => p.IsDeleted 
                        && p.DeletionTime != null
                        && p.DeletionTime.Value.AddDays(deletedEntityLifetime) < DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    private async Task<List<Volunteer>> GetVolunteerWithDeletedPets(CancellationToken cancellationToken)
    {
        return await dbContext.Volunteers
            .Include(v => v.Pets)
            .Where(v => v.Pets.Any(p => p.IsDeleted))
            .ToListAsync(cancellationToken);
    }
}