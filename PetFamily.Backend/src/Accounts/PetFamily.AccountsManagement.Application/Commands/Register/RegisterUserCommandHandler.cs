﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Infrastructure.Authentication;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.Commands.Register;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private UserManager<User> _userManager;
    private ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(UserManager<User> userManager, ILogger<RegisterUserCommandHandler> logger)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<UnitResult<ErrorList>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existedUser = await _userManager.FindByEmailAsync(command.Email);
        if (existedUser is not null)
            return Errors.General.AlreadyExists(command.Email).ToErrorList();

        var user = new User
        {
            UserName = command.UserName,
            Email = command.Email
        };

        var createUserResult = await _userManager.CreateAsync(user, command.Password);
        if (createUserResult.Succeeded)
        {
            _logger.LogInformation("Created user with id - '{id}'", user.Id);
            
            return UnitResult.Success<ErrorList>();
        }

        var errors = createUserResult.Errors
            .Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return new ErrorList(errors);
    }
}