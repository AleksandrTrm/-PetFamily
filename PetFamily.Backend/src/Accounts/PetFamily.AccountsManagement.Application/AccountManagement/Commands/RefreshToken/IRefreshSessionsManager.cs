using CSharpFunctionalExtensions;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.RefreshToken;

public interface IRefreshSessionsManager
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken);

    void Delete(RefreshSession refreshSession);
}