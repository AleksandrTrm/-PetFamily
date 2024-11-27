using Microsoft.AspNetCore.Authorization;
using PetFamily.AccountsManagement.Infrastructure.Requirements;

namespace PetFamily.AccountsManagement.Infrastructure;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var permission = context.User.Claims.FirstOrDefault(c => c.Type == "Permission");
        if (permission is null)
            return;

        if (permission.Value == requirement.Code)
            context.Succeed(requirement);
    }
}