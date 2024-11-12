using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid Id, Guid PetId, Status Status);