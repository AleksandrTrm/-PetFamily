﻿namespace PetFamily.Domain.Entities.SpeciesAggregate.Breeds;

public class BreedId
{
    //ef core
    private BreedId()
    {
    }
    
    private BreedId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; private set; }

    public static BreedId NewBreedId() => new(Guid.NewGuid());

    public static BreedId Empty() => new(Guid.Empty);

    public static BreedId Create(Guid id) => new(id);
}