using CSharpFunctionalExtensions;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Contracts.Responses;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Framework.Authorization;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.Commands.RefreshToken;

public class RefreshSessionHandler(
    IRefreshSessionsManager refreshSessionsManager,
    IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider) 
    : ICommandHandler<LoginResponse, RefreshSessionCommand>
{
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshSessionCommand command, CancellationToken cancellationToken)
    {
        var refreshSession = await refreshSessionsManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);
        
        if (refreshSession.IsFailure)
            return refreshSession.Error.ToErrorList();

        if (refreshSession.Value.ExpiresIn < DateTime.UtcNow)
            return Errors.Accounts.ExpiredToken().ToErrorList();

        var userClaimsResult = await tokenProvider.GetUserClaimsFromJwtToken(command.AccessToken);
        if (userClaimsResult.IsFailure)
            return userClaimsResult.Error.ToErrorList();

        var userIdString = userClaimsResult.Value.FirstOrDefault(c => c.Type == CustomClaims.SUB)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
            return Errors.General.NotFound().ToErrorList();

        if (userId != refreshSession.Value.UserId)
            return Errors.Accounts.InvalidJwtToken().ToErrorList();

        var tokenJtiString = userClaimsResult.Value.FirstOrDefault(c => c.Type == CustomClaims.JTI)?.Value;
        if (!Guid.TryParse(tokenJtiString, out var tokenJti))
            return Errors.General.NotFound().ToErrorList();

        if (tokenJti != refreshSession.Value.Jti)
            return Errors.Accounts.InvalidJwtToken().ToErrorList();

        refreshSessionsManager.Delete(refreshSession.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var accessTokenResponse = tokenProvider.GenerateAccessToken(refreshSession.Value.User);
        var refreshToken = await tokenProvider
            .GenerateRefreshToken(refreshSession.Value.User, accessTokenResponse.Jti, cancellationToken);

        return new LoginResponse(accessTokenResponse.JwtToken, refreshToken);
    }
}