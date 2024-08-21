using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.VolunteerValueObjects;

public record SocialMedias
{
    private SocialMedias()
    {
    }
    
    public SocialMedias(List<SocialMedia> socialMedias)
    {
        Value = socialMedias;
    }
    
    public List<SocialMedia> Value { get; }
}