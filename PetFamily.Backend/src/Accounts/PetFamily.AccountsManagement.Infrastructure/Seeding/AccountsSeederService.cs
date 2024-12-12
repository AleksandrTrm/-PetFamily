using System.Text.Json;
using PetFamily.Shared.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Infrastructure.Managers;
using PetFamily.AccountsManagement.Infrastructure.Managers.Options;

namespace PetFamily.AccountsManagement.Infrastructure.Seeding;

public class AccountsSeederService(
    RolePermissionsManager rolePermissionsManager, 
    RoleManager<Role> roleManager,
    PermissionManager permissionManager,
    AdminAccountsManager adminAccountsManager,
    UserManager<User> userManager,
    ILogger<AccountsSeederService> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var json = await File.ReadAllTextAsync(FilesPaths.ACCOUNTS, cancellationToken);

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json) 
                       ?? throw new ApplicationException("Could not deserialize role permission config.");
        
        await SeedPermissions(seedData);
        await SeedRoles(seedData);
        await SeedRolesPermissions(seedData);
        await SeedAdmin(cancellationToken);
    }

    private async Task SeedAdmin(CancellationToken cancellationToken) =>
        await adminAccountsManager.CreateAdminAccount(roleManager, userManager, cancellationToken);
    
    private async Task SeedRolesPermissions(RolePermissionOptions seedData)
    {
        var addedRolesPermissionsCount = await rolePermissionsManager.AddRangeIfExist(seedData.Roles);

        logger.LogInformation($"{addedRolesPermissionsCount} roles permissions has been added");
    }
    
    private async Task SeedRoles(RolePermissionOptions seedData)
    {
        var rolesToAdd = seedData.Roles.Select(r => r.Key);
        
        foreach (var roleName in rolesToAdd)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is not null)
                continue;

            await roleManager.CreateAsync(new Role
            { 
                Name = roleName
            });
        }
        
        logger.LogInformation("Roles has been seeded");
    }
    
    private async Task SeedPermissions(RolePermissionOptions seedData)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);
        
        var addedPermissionCount = await permissionManager.AddRangeIfExist(permissionsToAdd);

        logger.LogInformation($"{addedPermissionCount} permissions has been added");
    } 
}