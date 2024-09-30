using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entitites;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Domain.SpeciesManagement.AggregateRoot;

public class Species : Shared.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    //ef core
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public Species(SpeciesId id, SpeciesValue value) : base(id)
    {
        Value = value;
    }

    public IReadOnlyList<Breed> Breeds => _breeds;
    
    public SpeciesValue Value { get; }

    public Breed? FindBreed(Breed breed)
    {
        return _breeds.FirstOrDefault(b => b.Value.Value == breed.Value.Value);
    }

    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }
}