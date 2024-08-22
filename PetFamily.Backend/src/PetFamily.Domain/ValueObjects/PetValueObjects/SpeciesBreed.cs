using PetFamily.Domain.Entities.SpeciesAggregate.Breeds;
using PetFamily.Domain.Entities.SpeciesAggregate.Species;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record SpeciesBreed
{
    private SpeciesBreed()
    {
    }

    public SpeciesBreed(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; private set; }

    public BreedId BreedId { get; private set; }
}