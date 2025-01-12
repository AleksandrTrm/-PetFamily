using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Domain.Entities.Accounts;

public class VolunteerAccount
{
    public Guid Id { get; private set; }

    public int Experience { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public IReadOnlyList<Requisite> Requisites { get; private set; } = [];
    
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; private set; } = [];
}