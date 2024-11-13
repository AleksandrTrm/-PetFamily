namespace PetFamily.VolunteersManagement.Presentation.Requests.Write;

public record DeletePetFilesRequest(IEnumerable<Guid> FilesNames);