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
        UserId = user.Id;
        User = user;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    public void SetSocialNetworks(IEnumerable<SocialNetwork> socialNetworks) => 
        User.SetSocialNetworks(socialNetworks);

    public void SetPhoto(string photo) => User.SetPhoto(photo);
}