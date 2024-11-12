namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.DeletePet;

public record DeletePetCommand(Guid Id, Guid PetId);