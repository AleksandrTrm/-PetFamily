using Microsoft.AspNetCore.Mvc;
using PetFamily.AccountsManagement.Application.Commands.Login;
using PetFamily.Shared.Framework;
using PetFamily.Shared.Framework.Extensions;
using PetFamily.AccountsManagement.Presentation.Requests;
using PetFamily.AccountsManagement.Application.Commands.Register;

namespace PetFamily.AccountsManagement.Presentation;

public class AccountsController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var registerUserResult = await handler.Handle(request.ToCommand(), cancellationToken);
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
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}