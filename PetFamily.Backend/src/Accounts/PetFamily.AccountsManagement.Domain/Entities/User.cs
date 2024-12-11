using Microsoft.AspNetCore.Identity;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Domain.Entities;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];
    private List<SocialNetwork> _socialNetworks = [];

    private User() : base()
    {
    }

    public FullName FullName { get; set; }
    
    public string Photo { get; private set; }

    public IReadOnlyList<Role> Roles => _roles;

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public static User CreateUser(string userName, string email, FullName fullName, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            FullName = fullName,
            _roles = [role]
        };
    }

    public void SetSocialNetworks(IEnumerable<SocialNetwork> socialNetworks) =>
        _socialNetworks = socialNetworks.ToList();

    public void SetPhoto(string photo) => Photo = photo;
}