namespace PetFamily.Application.Features.Commands.Volunteers.Pet.DeletePetFiles;

public record DeletePetFilesCommand(Guid VolunteerId, Guid PetId, IEnumerable<Guid> FilesNames);