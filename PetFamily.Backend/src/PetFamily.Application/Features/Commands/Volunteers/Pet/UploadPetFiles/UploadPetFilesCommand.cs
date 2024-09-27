using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.UploadPetFiles;

public record UploadPetFilesCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileCommand> Files) : ICommand;

public record UploadFileCommand(Stream Content, string FileName);