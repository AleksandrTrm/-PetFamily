using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.VolunteerValueObjects;

public record SocialMedias
{
    //ef core
    private SocialMedias()
    {
    }
    
    public SocialMedias(List<SocialMedia> value)
    {
        Value = value;
    }
    
    public List<SocialMedia> Value { get; }
}