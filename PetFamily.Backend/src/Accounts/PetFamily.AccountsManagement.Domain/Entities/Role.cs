using Microsoft.AspNetCore.Identity;

namespace PetFamily.AccountsManagement.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public IEnumerable<RolePermission> RolePermissions { get; set; }

    public IEnumerable<User> Users { get; set; }
}