using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid Id, Guid PetId, Status Status);