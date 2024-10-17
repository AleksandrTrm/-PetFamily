using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Create;
using PetFamily.API.Controllers.Species.Get;
using PetFamily.Application.Features.Commands.SpeciesManagement.CreateBreed;
using PetFamily.Application.Features.Commands.SpeciesManagement.CreateSpecies;
using PetFamily.Application.Features.Commands.SpeciesManagement.DeleteBreed;
using PetFamily.Application.Features.Commands.SpeciesManagement.DeleteSpecies;
using PetFamily.Application.Features.Queries.Pets;
using PetFamily.Application.Features.Queries.Pets.GetBreedsBySpeciesId;
using PetFamily.Application.Features.Queries.Pets.GetSpeciesWithPagination;

namespace PetFamily.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetSpeciesWithPagination(
        CancellationToken cancellationToken,
        [FromQuery] GetSpeciesRequest request,
        [FromServices] GetSpeciesWithPaginationQueryHandler handler)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("breeds")]
    public async Task<ActionResult> GetBreedsBySpeciesId(
        [FromQuery] GetBreedsBySpeciesIdRequest request,
        [FromServices] GetBreedsBySpeicesIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        return Ok(result);
    }
    
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

    [HttpPost("breeds")]
    public async Task<ActionResult> CreateBreed(
        [FromBody] CreateBreedRequest request,
        [FromServices] CreateBreedCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var createBreedResult = await handler.Handle(request.ToCommand(), cancellationToken);
        if (createBreedResult.IsFailure)
            return BadRequest(createBreedResult.Error);

        return Ok(createBreedResult.Value);
    }

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