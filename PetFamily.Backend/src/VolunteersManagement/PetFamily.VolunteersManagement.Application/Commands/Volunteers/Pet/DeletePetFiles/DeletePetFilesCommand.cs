namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.DeletePetFiles;

public record DeletePetFilesCommand(Guid VolunteerId, Guid PetId, IEnumerable<Guid> FilesNames);