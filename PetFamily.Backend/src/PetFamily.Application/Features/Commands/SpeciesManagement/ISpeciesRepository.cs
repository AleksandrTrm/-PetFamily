using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;
using PetFamily.Domain.SpeciesManagement.Entitites;

namespace PetFamily.Application.Features.Commands.SpeciesManagement;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> CreateSpecies(
        Species species, 
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> CreateBreed(
        SpeciesId speciesId,
        Breed breed,
        CancellationToken cancellationToken = default);
}