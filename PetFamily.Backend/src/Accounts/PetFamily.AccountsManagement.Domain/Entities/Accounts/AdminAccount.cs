namespace PetFamily.AccountsManagement.Domain.Entities.Accounts;

public class AdminAccount
{
    public const string ROLE_NAME = "Admin";

    //ef core
    private AdminAccount()
    {
    }
    
    public AdminAccount(User user)
    {
        Guid.NewGuid();
        UserId = user.Id;
        User = user;
    }
    
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }
}