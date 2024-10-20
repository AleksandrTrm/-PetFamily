﻿using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Get.Requests;
using PetFamily.API.Controllers.Volunteers.Write.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.API.Response;
using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.VolunteerDtos;
using PetFamily.Application.Features.Commands.Volunteers.Create;
using PetFamily.Application.Features.Commands.Volunteers.Delete;
using PetFamily.Application.Features.Commands.Volunteers.Pet.AddPet;
using PetFamily.Application.Features.Commands.Volunteers.Pet.UploadPetFiles;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateMainInfo;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateRequisites;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateSocialMedias;
using PetFamily.Application.Features.Queries.Volunteers.GetFilteredVolunteersWithPagination;
using PetFamily.Application.Features.Queries.Volunteers.GetVolunteerById;
using UpdateVolunteerMainInfoDto = PetFamily.Application.DTOs.VolunteerDtos.UpdateVolunteerMainInfoDto;

namespace PetFamily.API.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    [HttpGet("volunteers")]
    public async Task<ActionResult> GetVolunteers(
        [FromQuery] GetVolunteersRequest request,
        [FromServices] GetFilteredVolunteersWithPaginationQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("volunteer/{id:guid}")]
    public async Task<ActionResult> GetVolunteerById(
        [FromRoute] Guid id,
        [FromServices] GetVolunteerByIdQueryHandler handler,
        CancellationToken cancellationToken)
        {
        var result = await handler.Handle(new GetVolunteerByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddPetCommand(
            id,
            request.Nickname,
            request.Description,
            request.Color,
            request.HealthInfo,
            request.SpeciesBreed,
            request.Address,
            request.Weight,
            request.Height,
            request.OwnerPhone,
            request.IsCastrated,
            request.DateOfBirth,
            request.IsVaccinated,
            request.Status,
            request.Requisites);

        var addPetResult = await handler.Handle(command, cancellationToken);
        if (addPetResult.IsFailure)
            return addPetResult.Error.ToResponse();

        return Ok(addPetResult.Value);
    }

    [HttpPost("{id:guid}/pet/photos")]
    public async Task<ActionResult> UploadPetFiles(
        [FromRoute] Guid id,
        [FromForm] UploadPetFilesRequest request,
        [FromServices] UploadPetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var processor = new FileProcessor();
        var fileContents = processor.Process(request.Files);
        
        var command = request.ToCommand(id, fileContents);

        var fileUploadingResult = await handler.Handle(command, cancellationToken);
        if (fileUploadingResult.IsFailure)
            return fileUploadingResult.Error.ToResponse();
        
        return Ok(fileUploadingResult.Value);
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var createResult = await handler.Handle(request.ToCommand(), cancellationToken);
        if (createResult.IsFailure)
            return createResult.Error.ToResponse();
        
        return Ok(Envelope.Ok(createResult.Value));
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMainInfoCommand(id, dto);
        
        var updateResult = await handler.Handle(command, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToResponse();
        
        return Ok(updateResult.Value);
    }

    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromRoute] Guid id,
        [FromBody] IEnumerable<RequisiteDto> dto,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRequisitesCommand(id, dto);

        var updateResult = await handler.Handle(command, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToResponse();
        
        return Ok(updateResult.Value);
    }
    
    [HttpPatch("{id:guid}/social-medias")]
    public async Task<ActionResult<Guid>> UpdateSocialMedias(
        [FromRoute] Guid id,
        [FromBody] IEnumerable<SocialMediaDto> dto,
        [FromServices] UpdateSocialMediasHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSocialMediasCommand(id, dto);

        var updateResult = await handler.Handle(command, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToResponse();
        
        return Ok(updateResult.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);

        var deleteResult = await handler.Handle(command, cancellationToken);
        if (deleteResult.IsFailure)
            return deleteResult.Error.ToResponse();
        
        return Ok(deleteResult.Value);
    }
}