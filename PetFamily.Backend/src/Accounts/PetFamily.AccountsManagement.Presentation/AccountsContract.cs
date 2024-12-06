using CSharpFunctionalExtensions;
using PetFamily.AccountsManagement.Contracts;
using PetFamily.AccountsManagement.Infrastructure.Managers;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Presentation;

public class AccountsContract(PermissionManager permissionManager) : IAccountsContract
{
    public async Task<Result<IReadOnlyList<string>, Error>> GetPermissionsOfUserById(
        Guid id, CancellationToken cancellationToken = default) =>
            await permissionManager.GetPermissionsByUserId(id, cancellationToken);
}