using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AccountsManagement.Contracts;

namespace PetFamily.Shared.Framework.Authorization;

public class PermissionRequirementHandler(IServiceScopeFactory scopeFactory) : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionAttribute permission)
    {
        var claimId = context.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.SUB);
        if (claimId is null)
            return;

        if (!Guid.TryParse(claimId.Value, out var id))
            return;
        
        using var scope = scopeFactory.CreateScope();

        var accountsContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();
        var permissionsResult = await accountsContract.GetPermissionsOfUserById(id);
        if (permissionsResult.IsFailure)
        {
            context.Fail();
            return;
        }
        
        if (permissionsResult.Value.Contains(permission.Code))
            context.Succeed(permission); 
    }
}