using Microsoft.AspNetCore.Identity;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Domain.Entities;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];
    private List<SocialNetwork> _socialNetworks = [];

    private User() : base()
    {
    }

    public FullName FullName { get; private set; }
    
    public string Photo { get; private set; }

    public IReadOnlyList<Role> Roles => _roles;

    public Guid? VolunteerId { get; private set; }
    public VolunteerAccount? Volunteer { get; private set; }

    public Guid? ParticipantId { get; private set; }
    public ParticipantAccount? Participant { get; private set; }

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
    
    public void UpdateFullName(FullName fullName) => FullName = fullName;
    
    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks) =>
        _socialNetworks = socialNetworks.ToList();

    public void UpdatePhoto(string photo) => Photo = photo;
}