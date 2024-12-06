using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.AccountsManagement.Domain.Entities;

namespace PetFamily.AccountsManagement.Infrastructure.Managers;

public class RolePermissionsManager(AccountsDbContext accountsDbContext, RoleManager<Role> roleManager)
{
    public async Task<int> AddRangeIfExist(Dictionary<string, string[]> rolesPermissions)
    {
        int addedRolesPermissionsCount = 0;
        
        foreach (var rolePermissions in rolesPermissions)
        {
            var role = await roleManager.FindByNameAsync(rolePermissions.Key);

            foreach (var permissionCode in rolePermissions.Value)
            {
                var permission = await accountsDbContext.Permissions
                    .FirstOrDefaultAsync(p => p.Code == permissionCode);

                var rolePermission = await accountsDbContext.RolePermissions
                    .FindAsync(permission!.Id, role!.Id);
                if (rolePermission is not null)
                    continue;
                
                await accountsDbContext.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = role!.Id,
                    PermissionId = permission!.Id
                });
            }
        }

        await accountsDbContext.SaveChangesAsync();

        return addedRolesPermissionsCount;
    }
}