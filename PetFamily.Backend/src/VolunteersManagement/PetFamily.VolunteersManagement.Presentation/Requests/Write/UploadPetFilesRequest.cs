using Microsoft.AspNetCore.Http;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UploadPetFiles;

namespace PetFamily.VolunteersManagement.Presentation.Requests.Write;

public record UploadPetFilesRequest(IFormFileCollection Files)
{
    public UploadPetFilesCommand ToCommand(Guid volunteerId, Guid PetId, IEnumerable<UploadFileCommand> files) =>
         new(volunteerId, PetId, files);
}
