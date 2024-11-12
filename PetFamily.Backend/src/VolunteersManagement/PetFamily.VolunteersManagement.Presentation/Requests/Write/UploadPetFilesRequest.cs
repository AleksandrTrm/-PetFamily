using Microsoft.AspNetCore.Http;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UploadPetFiles;

namespace PetFamily.WebAPI.Controllers.Volunteers.Write.Requests;

public record UploadPetFilesRequest(IFormFileCollection Files)
{
    public UploadPetFilesCommand ToCommand(Guid volunteerId, Guid PetId, IEnumerable<UploadFileCommand> files) =>
         new(volunteerId, PetId, files);
}
