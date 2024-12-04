using System.Text.Json;
using PetFamily.Shared.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Infrastructure.Managers;
using PetFamily.AccountsManagement.Infrastructure.Managers.Configurations;

namespace PetFamily.AccountsManagement.Infrastructure;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory, ILogger<AccountsSeeder> logger)
{
    private readonly ILogger<AccountsSeeder> _logger = logger;

    public async Task SeedAsync()
    {
        var json = await File.ReadAllTextAsync(FilesPaths.ACCOUNTS);

        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json) 
                       ?? throw new ApplicationException("Could not deserialize role permission config.");
        
        using var scope = serviceScopeFactory.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        var rolePermissionsManager = scope.ServiceProvider.GetRequiredService<RolePermissionsManager>();
        
        await SeedPermissions(seedData, permissionManager);
        await SeedRoles(seedData, roleManager);
        await SeedRolesPermissions(seedData, rolePermissionsManager);
    }

    private async Task SeedRolesPermissions(
        RolePermissionConfig seedData, 
        RolePermissionsManager rolePermissionsManager)
    {
        var addedRolesPermissionsCount = await rolePermissionsManager.AddRangeIfExist(seedData.Roles);

        _logger.LogInformation($"{addedRolesPermissionsCount} roles permissions has been added");
    }
    
    private async Task SeedRoles(RolePermissionConfig seedData, RoleManager<Role> roleManager)
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
        
        _logger.LogInformation("Roles has been seeded");
    }
    
    private async Task SeedPermissions(RolePermissionConfig seedData, PermissionManager permissionManager)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);
        
        var addedPermissionCount = await permissionManager.AddRangeIfExist(permissionsToAdd);

        _logger.LogInformation($"{addedPermissionCount} permissions has been added");
    } 
}