using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;

namespace PetFamily.Domain.SpeciesManagement.Entitites.Species;

public class Species : Shared.Entity<SpeciesId>
{
    private List<Breed> _breeds;
    
    //ef core
    private Species(SpeciesId id) : base(id)
    {
    }
    
    private Species(SpeciesId id, string species) : base(id)
    {
        Value = species;
    }

    public IReadOnlyList<Breed> Breeds => _breeds;
    
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