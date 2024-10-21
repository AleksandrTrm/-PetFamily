namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record DeletePetFilesRequest(IEnumerable<Guid> FilesNames);