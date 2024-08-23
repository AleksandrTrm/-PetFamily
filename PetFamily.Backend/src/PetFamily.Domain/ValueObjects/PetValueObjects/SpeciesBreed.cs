using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.Species;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record SpeciesBreed
{
    private SpeciesBreed(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; private set; }

    public Guid BreedId { get; private set; }

    public static Result<SpeciesBreed> Create(SpeciesId speciesId, Guid breedId)
    {
        return new SpeciesBreed(speciesId, breedId);
    }
}