using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.Login;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.RefreshToken;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.Register;
using PetFamily.AccountsManagement.Application.AccountManagement.Queries.GetAccountInfo;
using PetFamily.Shared.Framework;
using PetFamily.Shared.Framework.Extensions;
using PetFamily.AccountsManagement.Contracts.Requests;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.Framework.Authorization;

namespace PetFamily.AccountsManagement.Presentation;

public class AccountsController : ApplicationController
{
    [HttpGet("test")]
    [Permission(Permissions.Volunteer.CREATE_VOLUNTEER)]
    public async Task<IActionResult> Test()
    {
        return Ok("Test");
    }
    
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
        
        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh(
        [FromServices] RefreshSessionHandler handler, 
        CancellationToken cancellationToken)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized();
        
        var command = new RefreshSessionCommand(Guid.Parse(refreshToken));
        var refreshSessionResult = await handler.Handle(command, cancellationToken);
        if (refreshSessionResult.IsFailure)
            return refreshSessionResult.Error.ToResponse();

        HttpContext.Response.Cookies.Append("refreshToken", refreshSessionResult.Value.RefreshToken.ToString());

        var envelope = Envelope.Ok(refreshSessionResult.Value);
        
        return Ok(envelope);
    }
}