using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.AccountsManagement.Contracts.Responses;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.Abstractions;

public interface ITokenProvider
{
    GenerateAccessTokenResponse GenerateAccessToken(User user);

    Task<Guid> GenerateRefreshToken(User user, Guid refreshTokenJit, CancellationToken cancellationToken);

    Task<Result<List<Claim>, Error>> GetUserClaimsFromJwtToken(string jwtToken);
}