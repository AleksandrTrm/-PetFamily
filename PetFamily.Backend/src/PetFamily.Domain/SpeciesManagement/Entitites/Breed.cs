using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Domain.SpeciesManagement.Entitites;

public class Breed : Shared.Entity<BreedId>
{
    //ef core
    private Breed(BreedId id) : base(id)
    {
    }
    
    public Breed(BreedId id, BreedValue value) : base(id)
    {
        Value = value;
    }
    
    public BreedValue Value { get; private set; }
}