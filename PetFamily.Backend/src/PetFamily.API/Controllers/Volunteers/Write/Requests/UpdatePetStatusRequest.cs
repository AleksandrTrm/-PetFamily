using PetFamily.Application.Features.Commands.Volunteers.Pet.UpdatePetStatus;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record UpdatePetStatusRequest(Status Status)
{
    public UpdatePetStatusCommand ToCommand(Guid id, Guid petId) =>
        new UpdatePetStatusCommand(id, petId, Status);
}