using PetFamily.Application.Volunteers.Pet.UploadPetFiles;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UploadPetFilesRequest(Guid PetId, IFormFileCollection Files)
{
    public UploadPetFilesCommand ToCommand(Guid volunteerId, IEnumerable<UploadFileCommand> files) =>
         new(volunteerId, PetId, files);
}
