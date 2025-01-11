using Microsoft.AspNetCore.Mvc;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.Login;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.RefreshToken;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.Register;
using PetFamily.AccountsManagement.Application.AccountManagement.Queries.GetAccountInfo;
using PetFamily.Shared.Framework;
using PetFamily.Shared.Framework.Extensions;
using PetFamily.AccountsManagement.Contracts.Requests;

namespace PetFamily.AccountsManagement.Presentation;

public class AccountsController : ApplicationController
{
    [HttpGet("accounts/{id:guid}")]
    public async Task<IActionResult> GetAccountInfo(
        [FromRoute] Guid id,
        [FromServices] GetAccountInfoQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetAccountInfoQuery(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.UserName, request.FullName, request.Password);
        
        var registerUserResult = await handler.Handle(command, cancellationToken);
        if (registerUserResult.IsFailure)
            return registerUserResult.Error.ToResponse();

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        [FromServices] RefreshSessionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RefreshSessionCommand(request.AccessToken, request.RefreshToken);
        var refreshSessionResult = await handler.Handle(command, cancellationToken);
        if (refreshSessionResult.IsFailure)
            return refreshSessionResult.Error.ToResponse();

        return Ok(refreshSessionResult.Value);
    }
}