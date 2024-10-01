using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Create;
using PetFamily.Application.Features.Commands.SpeciesManagement.CreateBreed;
using PetFamily.Application.Features.Commands.SpeciesManagement.CreateSpecies;

namespace PetFamily.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpPost("species")]
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
}