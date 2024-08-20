using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.VolunteerValueObjects;

public record SocialMedias
{
    private SocialMedias(List<SocialMedia> socialMedias)
    {
        Value = socialMedias;
    }
    
    public List<SocialMedia> Value { get; }

    public static Result<SocialMedias, string> Create(List<SocialMedia> socialMedias)
    {
        if (socialMedias.Count is < 1 or > 5)
            return "Volunteer can not have more than five and less than one social medias";

        return new SocialMedias(socialMedias);
    }
}