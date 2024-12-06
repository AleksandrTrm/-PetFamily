namespace PetFamily.AccountsManagement.Infrastructure.Managers.Configurations;

public class RolePermissionConfig
{
    public Dictionary<string, string[]> Roles { get; set; }

    public Dictionary<string, string[]> Permissions { get; set; }
}