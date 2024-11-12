using Microsoft.AspNetCore.Mvc;
using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.Framework;
using PetFamily.Shared.Framework.Extensions;
using PetFamily.Shared.Framework.Processors;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Create;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Delete;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.AddPet;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.DeletePet;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.DeletePetFiles;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.SetMainPhoto;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetMainInfo;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetStatus;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UploadPetFiles;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateMainInfo;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateRequisites;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateSocialMedias;
using PetFamily.VolunteersManagement.Application.Queries.Pets.GetFilteredPetsWithPagination;
using PetFamily.VolunteersManagement.Application.Queries.Pets.GetPetById;
using PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;
using PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetVolunteerById;
using PetFamily.WebAPI.Controllers.Volunteers.Get.Requests;
using PetFamily.WebAPI.Controllers.Volunteers.Write.Requests;

namespace PetFamily.WebAPI.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetVolunteers(
        [FromQuery] GetVolunteersRequest request,
        [FromServices] GetFilteredVolunteersWithPaginationQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
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

    [HttpGet("pets")]
    public async Task<ActionResult> GetPets(
        [FromServices] GetFilteredPetsWithPaginationQueryHandler handler,
        [FromQuery] GetPetsRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(request.ToQuery(), cancellationToken));
    }

    [HttpGet("pets/{id:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid id,
        [FromServices] GetPetByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetPetByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/pets")]
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

    [HttpPost("{id:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult> UploadPetFiles(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromForm] UploadPetFilesRequest request,
        [FromServices] UploadPetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var processor = new FileProcessor();
        var fileContents = processor.Process(request.Files);
        
        var command = request.ToCommand(id, petId, fileContents);

        var fileUploadingResult = await handler.Handle(command, cancellationToken);
        if (fileUploadingResult.IsFailure)
            return fileUploadingResult.Error.ToResponse();
        
        return Ok(fileUploadingResult.Value);
    }

    [HttpDelete("{id:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult> DeletePetPhotos(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] DeletePetFilesRequest request,
        [FromServices] DeletePetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var deletePetFilesResult = await handler
            .Handle(new DeletePetFilesCommand(id, petId, request.FilesNames), cancellationToken);
        if (deletePetFilesResult.IsFailure)
            return BadRequest(deletePetFilesResult.Error);

        return Ok(deletePetFilesResult.Value);
    }

    [HttpPut("{id:guid}/pets/{petId:guid}/main-info")]
    public async Task<ActionResult> UpdatePetMainInfo(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetMainInfoRequest request,
        [FromServices] UpdatePetMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePetMainInfoCommand(
            petId,
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

        var updatePetMainInfoResult = await handler.Handle(command, cancellationToken);
        if (updatePetMainInfoResult.IsFailure)
            return BadRequest(updatePetMainInfoResult.Error);

        return Ok(updatePetMainInfoResult.Value);
    }

    [HttpPatch("{id:guid}/pets/{petId:guid}/status")]
    public async Task<ActionResult> UpdatePetStatus(
        [FromRoute] Guid id, 
        [FromRoute] Guid petId,
        [FromForm] UpdatePetStatusRequest request,
        [FromServices] UpdatePetStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var updatePetStatusResult = await handler.Handle(request.ToCommand(id, petId), cancellationToken);
        if (updatePetStatusResult.IsFailure)
            return BadRequest(updatePetStatusResult.Error);

        return Ok(updatePetStatusResult.Value);
    }

    [HttpDelete("{id:guid}/pets/{petId:guid}")]
    public async Task<ActionResult> DeletePet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] DeletePetHandler handler,
        CancellationToken cancellationToken)
    {
        var deletePetResult = await handler.Handle(new DeletePetCommand(id, petId), cancellationToken);
        if (deletePetResult.IsFailure)
            return BadRequest(deletePetResult.Error);

        return Ok(deletePetResult.Value);
    }

    [HttpPatch("{id:guid}/pets/{petId:guid}/pet-photos/{name:guid}")]
    public async Task<ActionResult> SetPetMainPhoto(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromRoute] Guid name,
        [FromServices] SetMainPhotoHandler handler,
        CancellationToken cancellationToken)
    {
        var setMainPhotoResult = await handler.Handle(new SetMainPhotoCommand(id, petId, name), cancellationToken);
        if (setMainPhotoResult.IsFailure)
            return BadRequest(setMainPhotoResult.Error);

        return Ok(setMainPhotoResult.Value);
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(
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