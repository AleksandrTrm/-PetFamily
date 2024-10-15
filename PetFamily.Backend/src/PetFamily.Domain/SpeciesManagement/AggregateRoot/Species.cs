using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
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
    
    public Species(SpeciesId id, string name) : base(id)
    {
        Name = name;
    }

    public IReadOnlyList<Breed> Breeds => _breeds;
    
    public string Name { get; }

    public Breed? FindBreed(Breed breed)
    {
        return _breeds.FirstOrDefault(b => b.Name == breed.Name);
    }

    public UnitResult<Error> AddBreed(Breed breed)
    {
        if (_breeds.FirstOrDefault(b => b.Name == breed.Name) is not null)
            return Error.Conflict("record.already.exists", "This breed already exists");
        
        _breeds.Add(breed);

        return Result.Success<Error>();
    }
}