using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Infrastructure.Managers;

public class PermissionManager(AccountsDbContext accountsDbContext)
{
    public async Task<Result<IReadOnlyList<string>, Error>> GetPermissionsByUserId(Guid id, CancellationToken cancellationToken)
    {
        var user = await accountsDbContext.Users
            .Include(u => u.Roles)
            .ThenInclude(role => role.RolePermissions)
            .ThenInclude(rolePermission => rolePermission.Permission)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null)
            return Errors.General.NotFound(id, "user");

        var permissions = user.Roles
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission)
            .Select(p => p.Code)
            .ToList();
        
        return permissions;
    }

    public async Task<int> AddRangeIfExist(IEnumerable<string> permissionsCodes)
    {
        int savedPermissionsCount = 0;

        foreach (var permissionCode in permissionsCodes)
        {
            var isPermissionExists = await accountsDbContext.Permissions
                .AnyAsync(p => p.Code == permissionCode);

            if (isPermissionExists)
                continue;

            await accountsDbContext.Permissions.AddAsync(new Permission
            {
                Code = permissionCode
            });

            savedPermissionsCount++;
        }

        await accountsDbContext.SaveChangesAsync();

        return savedPermissionsCount;
    }
}