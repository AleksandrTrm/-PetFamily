using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Contracts.Responses;
using PetFamily.Shared.Core;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Framework.Authorization;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.RefreshToken;

public class RefreshSessionHandler(
    IRefreshSessionsManager refreshSessionsManager,
    [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider) 
    : ICommandHandler<LoginResponse, RefreshSessionCommand>
{
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshSessionCommand command, CancellationToken cancellationToken)
    {
        var oldRefreshSession = await refreshSessionsManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);
        
        if (oldRefreshSession.IsFailure)
            return oldRefreshSession.Error.ToErrorList();

        if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
            return Errors.Accounts.ExpiredToken().ToErrorList();

        refreshSessionsManager.Delete(oldRefreshSession.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var accessTokenResponse = tokenProvider.GenerateAccessToken(oldRefreshSession.Value.User);
        var refreshToken = await tokenProvider
            .GenerateRefreshToken(oldRefreshSession.Value.User, accessTokenResponse.Jti, cancellationToken);

        return new LoginResponse(
            accessTokenResponse.JwtToken, 
            refreshToken,
            oldRefreshSession.Value.User.Id,
            oldRefreshSession.Value.User.UserName!,
            oldRefreshSession.Value.User.Email!);
    }
}