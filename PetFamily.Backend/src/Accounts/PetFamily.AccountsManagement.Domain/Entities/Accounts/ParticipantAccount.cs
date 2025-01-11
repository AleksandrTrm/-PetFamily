using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Domain.Entities.Accounts;

public class ParticipantAccount
{
    public const string ROLE_NAME = "Participant";

    //ef core
    private ParticipantAccount()
    {
    }

    public ParticipantAccount(User user)
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }
}