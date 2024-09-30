using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Features.Commands.SpeciesManagement;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;
using PetFamily.Domain.SpeciesManagement.Entitites;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private WriteDbContext _context;

    public SpeciesRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid, Error>> CreateSpecies(
        Species species, 
        CancellationToken cancellationToken = default)
    {
        var getSpeciesResult = await _context.Species
            .FirstOrDefaultAsync(s => s.Value == species.Value, cancellationToken);
        if (getSpeciesResult is not null)
            return Errors.General.AlreadyExists(nameof(species));

        await _context.Species.AddAsync(species, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return species.Id.Value;
    }

    public async Task<Result<Guid, Error>> CreateBreed(
        SpeciesId speciesId,
        Breed breed,
        CancellationToken cancellationToken = default)
    {
        var getSpeciesResult = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);
        if (getSpeciesResult is null)
            return Errors.General.NotFound(speciesId.Value, "species");

        var getBreedResult = getSpeciesResult.FindBreed(breed);
        if (getBreedResult is not null)
            return Errors.General.AlreadyExists(nameof(breed));

        getSpeciesResult.AddBreed(breed);

        await _context.SaveChangesAsync(cancellationToken);

        return breed.Id.Value;
    }
}