using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;
using PetFamily.Domain.SpeciesManagement.Entitites;

namespace PetFamily.Application.Features.Commands.SpeciesManagement;

public interface ISpeciesRepository
{
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

    Task<Result<Guid, Error>> DeleteBreedById(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken = default);
}