using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.Breeds;
using Entity = PetFamily.Domain.Shared.Entity<PetFamily.Domain.Entities.SpeciesAggregate.Species.SpeciesId>;

namespace PetFamily.Domain.Entities.SpeciesAggregate.Species;

public class Species : Entity
{
    //ef core
    private Species(SpeciesId id) : base(id)
    {
    }
    
    private Species(SpeciesId id, string species) : base(id)
    {
        Value = species;
    }

    public List<Breed> Breeds { get; private set; }
    
    public string Value { get; }

    public static Result<Species, Error> Create(SpeciesId id, string species)
    {
        if (string.IsNullOrWhiteSpace(species))
            return Errors.General.InvalidValue(nameof(species));

        if (species.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_HIGH_LENGTH, nameof(species));

        return new Species(id, species);
    }
}