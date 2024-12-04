using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Infrastructure.Jwt.Options;
using PetFamily.Shared.Framework.Authorization;

namespace PetFamily.AccountsManagement.Infrastructure.Jwt;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }
    
    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = [
            new Claim(CustomClaims.SUB, user.Id.ToString()),
            new Claim(CustomClaims.EMAIL, user.Email ?? ""),
            new Claim("Permission", "get.species")
        ];
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutes)),
            signingCredentials: signingCredentials,
            claims: claims);

        var stringTokenResult = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return stringTokenResult;
    }
}