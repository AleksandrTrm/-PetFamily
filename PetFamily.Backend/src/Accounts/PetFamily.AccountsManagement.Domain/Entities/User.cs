using Microsoft.AspNetCore.Identity;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public FullName FullName { get; set; }
    
    public string Photo { get; set; }

    public IReadOnlyList<Role> Roles { get; set; }

    public IReadOnlyList<SocialNetwork> SocialNetworks { get; set; }
}