using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Contracts.Responses;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginResponse, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginCommandHandler> logger)
    {
        _tokenProvider = tokenProvider;
        _logger = logger;
        _userManager = userManager;
    }
    
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        LoginCommand command, 
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Errors.Accounts.InvalidCredentials().ToErrorList();

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
            return Errors.Accounts.InvalidCredentials().ToErrorList();

        var jwtTokenResponse = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, jwtTokenResponse.Jti,cancellationToken);

        _logger.LogInformation("Successfully logged in.");
        
        return new LoginResponse(jwtTokenResponse.JwtToken, refreshToken);
    }
}