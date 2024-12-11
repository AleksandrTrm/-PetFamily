namespace PetFamily.AccountsManagement.Infrastructure.Managers.Options;

public class RolePermissionOptions
{
    public Dictionary<string, string[]> Roles { get; set; }

    public Dictionary<string, string[]> Permissions { get; set; }
}