using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Contracts;

public interface IAccountsContract
{
    Task<Result<IReadOnlyList<string>, Error>> GetPermissionsOfUserById(Guid id,
        CancellationToken cancellationToken = default);
}