using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.IDs;

namespace PetFamily.BreedsManagement.Domain.Entitites;

public class Breed : Entity<BreedId>
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