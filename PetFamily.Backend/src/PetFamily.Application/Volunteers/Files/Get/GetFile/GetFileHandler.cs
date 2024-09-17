using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Files.Get.GetFile;

public class GetFileHandler
{
    private ILogger<GetFileHandler> _logger;
    private IFileProvider _fileProvider;

    public GetFileHandler(IFileProvider fileProvider, ILogger<GetFileHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(
        GetFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var fileData = new FileData(null, command.BucketName, command.Path);

        var result = await _fileProvider.GetFile(fileData, cancellationToken);

        return result.Value;
    }
}