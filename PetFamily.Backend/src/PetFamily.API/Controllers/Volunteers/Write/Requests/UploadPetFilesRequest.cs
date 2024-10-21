using PetFamily.Application.Features.Commands.Volunteers.Pet.UploadPetFiles;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record UploadPetFilesRequest(IFormFileCollection Files)
{
    public UploadPetFilesCommand ToCommand(Guid volunteerId, Guid PetId, IEnumerable<UploadFileCommand> files) =>
         new(volunteerId, PetId, files);
}
