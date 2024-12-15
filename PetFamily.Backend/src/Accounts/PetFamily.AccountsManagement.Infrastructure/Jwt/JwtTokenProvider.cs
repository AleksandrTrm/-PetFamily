using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CSharpFunctionalExtensions;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Contracts.Responses;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.Core.Options;
using PetFamily.Shared.Framework.Authorization;
using PetFamily.Shared.SharedKernel.Error;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PetFamily.AccountsManagement.Infrastructure.Jwt;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private AccountsDbContext _dbContext;

    public JwtTokenProvider(
        IOptions<JwtOptions> options, 
        AccountsDbContext dbContext)
    {
        _dbContext = dbContext;
        _jwtOptions = options.Value;
    }
    
    public GenerateAccessTokenResponse GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jti = Guid.NewGuid();
        
        Claim[] claims = [
            new Claim(CustomClaims.SUB, user.Id.ToString()),
            new Claim(CustomClaims.EMAIL, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
        ];
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutes)),
            signingCredentials: signingCredentials,
            claims: claims);

        var JwtTokenStringResult = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return new GenerateAccessTokenResponse(JwtTokenStringResult, jti);
    }

    public async Task<Result<List<Claim>, Error>> GetUserClaimsFromJwtToken(string jwtToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var validationResult = await jwtSecurityTokenHandler
            .ValidateTokenAsync(jwtToken, TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions));
        if (validationResult.IsValid is false)
            return Errors.Accounts.InvalidJwtToken();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }
    
    public async Task<Guid> GenerateRefreshToken(
        User user,
        Guid refreshTokenJti,
        CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            Jti = refreshTokenJti, 
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(30),
            RefreshToken = Guid.NewGuid()
        };

        _dbContext.RefreshSessions.Add(refreshSession);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    } 
}