using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Domain.SpeciesManagement.Breeds;

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

    public static Result<Breed, Error> Create(BreedId id, string breed, SpeciesId speciesId)
    {
        if (string.IsNullOrWhiteSpace(breed))
            return Errors.General.InvalidValue(nameof(breed));

        if (breed.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_HIGH_LENGTH, nameof(breed));

        return new Breed(id, breed, speciesId);
    }
}