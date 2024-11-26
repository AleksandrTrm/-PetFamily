using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Shared.Framework;
using PetFamily.BreedsManagement.Application.Commands.CreateBreed;
using PetFamily.BreedsManagement.Application.Commands.CreateSpecies;
using PetFamily.BreedsManagement.Application.Commands.DeleteBreed;
using PetFamily.BreedsManagement.Application.Commands.DeleteSpecies;
using PetFamily.BreedsManagement.Application.Queries.GetBreedsBySpeciesId;
using PetFamily.BreedsManagement.Application.Queries.GetSpeciesWithPagination;
using PetFamily.BreedsManagement.Presentation.Requests;

namespace PetFamily.BreedsManagement.Presentation;

public class SpeciesController : ApplicationController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetSpeciesWithPagination(
        CancellationToken cancellationToken,
        [FromQuery] GetSpeciesRequest request,
        [FromServices] GetSpeciesWithPaginationQueryHandler handler)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("breeds")]
    public async Task<ActionResult> GetBreedsBySpeciesId(
        [FromQuery] GetBreedsBySpeciesIdRequest request,
        [FromServices] GetBreedsBySpeciesIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateSpecies(
        [FromBody] CreateSpeciesRequest request,
        [FromServices] CreateSpeciesCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var createSpeciesResult = await handler.Handle(request.ToCommand(), cancellationToken);
        if (createSpeciesResult.IsFailure)
            return BadRequest(createSpeciesResult.Error);

        return Ok(createSpeciesResult.Value);
    }

    [Authorize]
    [HttpPost("{id:guid}/breeds")]
    public async Task<ActionResult> CreateBreed(
        [FromRoute] Guid id,
        [FromBody] string breed,
        [FromServices] CreateBreedCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateBreedCommand(id, breed);
        
        var createBreedResult = await handler.Handle(command, cancellationToken);
        if (createBreedResult.IsFailure)
            return BadRequest(createBreedResult.Error);

        return Ok(createBreedResult.Value);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteSpecies(
        [FromRoute] Guid id,
        [FromServices] DeleteSpeciesByIdCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeleteSpeciesByIdCommand(id), cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpDelete("{id:guid}/breeds/{breedId:guid}")]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] Guid id,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var deleteBreedResult = await handler.Handle(new DeleteBreedCommand(id, breedId), cancellationToken);
        if (deleteBreedResult.IsFailure)
            return BadRequest(deleteBreedResult.Error);

        return Ok(deleteBreedResult.Value);
    }
}