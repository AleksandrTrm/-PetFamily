using FluentValidation;
using PetFamily.API.Response;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.DTOs;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Volunteers.Update.UpdateRequisites;
using PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        [FromServices] IValidator<UpdateMainInfoRequest> validator,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new UpdateMainInfoRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            validationResult.ToValidationErrorResponse();
        
        var result = await handler.Handle(request, cancellationToken);
        
        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromRoute] Guid id,
        [FromBody] RequisitesDto dto,
        [FromServices] IValidator<UpdateRequisitesRequest> validator,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new UpdateRequisitesRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var updateResult = await handler.Handle(request, cancellationToken);

        return Ok(updateResult.Value);
    }
    
    [HttpPatch("{id:guid}/social-medias")]
    public async Task<ActionResult<Guid>> UpdateSocialMedias(
        [FromRoute] Guid id,
        [FromBody] SocialMediasDto dto,
        [FromServices] IValidator<UpdateSocialMediasRequest> validator,
        [FromServices] UpdateSocialMediasHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new UpdateSocialMediasRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var updateResult = await handler.Handle(request, cancellationToken);

        return Ok(updateResult.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var updateResult = await handler.Handle(request, cancellationToken);

        return Ok(updateResult.Value);
    }
}