namespace PetFamily.Application.Features.Commands.Volunteers.Pet.SetMainPhoto;

public record SetMainPhotoCommand(Guid Id, Guid PetId, Guid Name);