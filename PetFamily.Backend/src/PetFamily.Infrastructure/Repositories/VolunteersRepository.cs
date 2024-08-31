using CSharpFunctionalExtensions;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.VolunteersManagement.Volunteer;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private ApplicationDbContext _context;
    
    public VolunteersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Create(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return (Guid)volunteer.Id;
    }
}