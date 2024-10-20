namespace PetFamily.Application.Features.Commands.Volunteers.Pet.DeletePet;

public record DeletePetCommand(Guid Id, Guid PetId);