using FluentValidation;
using PetFamily.API.Response;
using PetFamily.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            var validationErrors = validationResult.Errors;

            var responseErrors = from validationError in validationErrors
                let errorMessage = validationError.ErrorMessage
                let error = Error.Deserialize(errorMessage)
                select new ResponseError(error.Code, error.Message, validationError.PropertyName);
            
            var envelope = Envelope.Error(responseErrors);

            return BadRequest(envelope);
        }

        var result = await handler.Handle(request, cancellationToken);

        return Ok(Envelope.Ok(result.Value));
    }
}