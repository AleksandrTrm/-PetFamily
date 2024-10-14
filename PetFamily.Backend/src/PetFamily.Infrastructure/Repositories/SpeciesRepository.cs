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

    public async Task<Result<Breed, Error>> GetBreedByName(Guid speciesId, string name)
    {
        var species = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == SpeciesId.Create(speciesId));
        if (species is null)
            return Errors.General.NotFound(speciesId);

        var breed = species.Breeds.FirstOrDefault(b => b.Name == name);
        if (breed is null)
            return Errors.General.NotFound();

        return breed;
    }

    public async Task<Result<Species, Error>> GetSpeciesByName(string name)
    {
        var species = await _context.Species.FirstOrDefaultAsync(s => s.Name == name);
        if (species is null)
            return Errors.General.NotFound();

        return species;
    }
    
    public async Task<Result<Guid, Error>> CreateSpecies(
        Species species, 
        CancellationToken cancellationToken = default)
    {
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

        getSpeciesResult.AddBreed(breed);

        await _context.SaveChangesAsync(cancellationToken);

        return breed.Id.Value;
    }
}