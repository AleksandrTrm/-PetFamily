using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Domain.SpeciesManagement.Entitites;

public class Breed : Shared.Entity<BreedId>
{
    //ef core
    private Breed(BreedId id) : base(id)
    {
    }
    
    public Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
}