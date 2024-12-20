﻿using CSharpFunctionalExtensions;
using PetFamily.BreedsManagement.Domain.Entitites;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;

namespace PetFamily.BreedsManagement.Domain.AggregateRoot;

public class Species : Shared.SharedKernel.Entity<SpeciesId>
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

    public Breed? FindBreed(string name) =>
        _breeds.FirstOrDefault(b => b.Name == name);

    public UnitResult<Error> AddBreed(Breed breed)
    {
        if (_breeds.FirstOrDefault(b => b.Name == breed.Name) is not null)
            return Error.Conflict("record.already.exists", "This breed already exists");
        
        _breeds.Add(breed);

        return Result.Success<Error>();
    }

    public void RemoveBreed(Breed breed) =>
        _breeds.Remove(breed);
}