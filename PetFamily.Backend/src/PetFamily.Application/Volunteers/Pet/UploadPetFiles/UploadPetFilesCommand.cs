using System.Collections;

namespace PetFamily.Application.Volunteers.Pet.UploadPetFiles;

public record UploadPetFilesCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileCommand> Files);

public record UploadFileCommand(Stream Content, string FileName);