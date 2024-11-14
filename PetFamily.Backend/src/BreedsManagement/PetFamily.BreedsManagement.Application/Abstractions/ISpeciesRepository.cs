using CSharpFunctionalExtensions;
using PetFamily.BreedsManagement.Domain.AggregateRoot;
using PetFamily.BreedsManagement.Domain.Entitites;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;

namespace PetFamily.BreedsManagement.Application.Abstractions;

public interface ISpeciesRepository
{
    Task SaveChanges(Species species, CancellationToken cancellationToken);
    
    Task<Result<Species, Error>> GetSpeciesById(Guid speciesId, CancellationToken cancellationToken);
    
    Task<Result<Breed, Error>> GetBreedByName(Guid speciesId, string name);
    
    Task<Result<Species, Error>> GetSpeciesByName(string name);
    
    Task<Result<Guid, Error>> CreateSpecies(
        Species species, 
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> CreateBreed(
        SpeciesId speciesId,
        Breed breed,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> DeleteSpeciesById(Guid id, CancellationToken cancellationToken = default);
}