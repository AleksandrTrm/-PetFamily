using Microsoft.AspNetCore.Identity;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.Infrastructure.Authentication;

public class User : IdentityUser<Guid>
{
    //public IReadOnlyList<SocialMedia> SocialMedias { get; set; }

    //public string Photo { get; set; }
}