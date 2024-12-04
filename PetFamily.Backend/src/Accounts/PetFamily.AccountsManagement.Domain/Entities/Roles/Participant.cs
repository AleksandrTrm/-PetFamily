namespace PetFamily.AccountsManagement.Domain.Entities.Roles;

public class Participant
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}