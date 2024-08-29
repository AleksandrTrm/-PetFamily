﻿using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.VolunteerValueObjects;

public record SocialMedias
{
    //ef core
    private SocialMedias()
    {
    }
    
    public SocialMedias(IEnumerable<SocialMedia> value)
    {
        Value = value.ToList();
    }
    
    public IReadOnlyList<SocialMedia> Value { get; }
}