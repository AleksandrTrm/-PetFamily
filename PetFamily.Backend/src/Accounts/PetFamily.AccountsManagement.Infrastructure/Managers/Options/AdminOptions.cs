namespace PetFamily.AccountsManagement.Infrastructure.Managers.Options;

public class AdminOptions
{
    public const string ADMIN = "ADMIN";

    public string Email { get; init; }

    public string UserName { get; init; }

    public string Name { get; init; }

    public string Surname { get; init; }

    public string Patronymic { get; init; }

    public string Password { get; init; }
}