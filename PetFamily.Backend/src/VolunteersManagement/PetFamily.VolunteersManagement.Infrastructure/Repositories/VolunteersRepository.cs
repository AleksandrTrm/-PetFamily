using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.VolunteersManagement.Application.Abstractions;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;
using PetFamily.VolunteersManagement.Infrastructure.DbContexts;

namespace PetFamily.VolunteersManagement.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private WriteDbContext _context;

    public VolunteersRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid, Error>> Create(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        var volunteerResult = await GetByPhoneNumber(volunteer.PhoneNumber, cancellationToken);
        if (volunteerResult.IsSuccess)
            return Errors.General.AlreadyExists("PhoneNumber");
        
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return (Guid)volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId);

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (volunteerResult is null)
            return Errors.General.NotFound();

        return volunteerResult;
    }

    public async Task<Result<Guid, Error>> SaveChanges(
        Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Attach(volunteer);
        await _context.SaveChangesAsync(cancellationToken);

        return (Guid)volunteer.Id;
    }
}