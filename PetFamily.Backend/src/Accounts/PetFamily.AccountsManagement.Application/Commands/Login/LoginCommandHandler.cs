using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.Infrastructure.Authentication;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.Commands.Login;

public class LoginCommandHandler : ICommandHandler<string, LoginCommand>
{
    private UserManager<User> _userManager;
    private ITokenProvider _tokenProvider;
    private ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginCommandHandler> logger)
    {
        _tokenProvider = tokenProvider;
        _logger = logger;
        _userManager = userManager;
    }
    
    public async Task<Result<string, ErrorList>> Handle(
        LoginCommand command, 
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Errors.Accounts.InvalidCredentials().ToErrorList();

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
            return Errors.Accounts.InvalidCredentials().ToErrorList();

        var token = _tokenProvider.GenerateAccessToken(user);

        _logger.LogInformation("Successfully logged in.");
        
        return token;
    }
}