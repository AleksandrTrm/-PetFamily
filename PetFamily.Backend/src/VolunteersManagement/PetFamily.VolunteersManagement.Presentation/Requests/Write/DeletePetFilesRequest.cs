namespace PetFamily.WebAPI.Controllers.Volunteers.Write.Requests;

public record DeletePetFilesRequest(IEnumerable<Guid> FilesNames);