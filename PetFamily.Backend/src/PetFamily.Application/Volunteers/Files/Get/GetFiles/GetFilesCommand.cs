using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Files.Get.GetFile;

namespace PetFamily.Application.Volunteers.Files.Get.GetFiles;

public record GetFilesCommand(IEnumerable<GetFileCommand> GetFilesRequests);