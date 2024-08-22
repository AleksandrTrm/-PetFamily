using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.SpeciesAggregate.Breeds;

public class Breed : Shared.Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    private Breed(BreedId id, string breed, SpeciesId speciesId) : base(id)
    {
        Value = breed;
    }
    
    public string Value { get; private set; }

    public static Result<Breed, string> Create(BreedId id, string breed, SpeciesId speciesId)
    {
        if (string.IsNullOrWhiteSpace(breed))
            return "Breed can not be empty";

        if (breed.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return $"The count of characters of breed can not be more than {Constants.MAX_MIDDLE_HIGH_LENGTH}";

        return new Breed(id, breed, speciesId);
    }
}