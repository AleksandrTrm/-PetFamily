using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetStatus;
using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;

namespace PetFamily.VolunteersManagement.Presentation.Requests.Write;

public record UpdatePetStatusRequest(Status Status)
{
    public UpdatePetStatusCommand ToCommand(Guid id, Guid petId) =>
        new UpdatePetStatusCommand(id, petId, Status);
}