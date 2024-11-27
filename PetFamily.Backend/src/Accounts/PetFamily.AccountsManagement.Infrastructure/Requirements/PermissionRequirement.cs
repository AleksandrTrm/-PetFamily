using Microsoft.AspNetCore.Authorization;

namespace PetFamily.AccountsManagement.Infrastructure.Requirements;

public class PermissionRequirement(string code) : IAuthorizationRequirement
{
    public string Code { get; } = code;
}