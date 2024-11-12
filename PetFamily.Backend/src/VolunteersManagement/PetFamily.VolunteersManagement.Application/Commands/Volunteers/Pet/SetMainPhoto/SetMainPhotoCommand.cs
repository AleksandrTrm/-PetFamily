namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.SetMainPhoto;

public record SetMainPhotoCommand(Guid Id, Guid PetId, Guid Name);