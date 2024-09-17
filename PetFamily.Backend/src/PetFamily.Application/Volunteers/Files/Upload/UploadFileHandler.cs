using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Files.Upload;

public class UploadFileHandler
{
    private ILogger<UploadFileHandler> _logger;
    private IFileProvider _fileProvider;

    public UploadFileHandler(IFileProvider fileProvider, ILogger<UploadFileHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(
        UploadFileCommand command, 
        CancellationToken cancellationToken = default)
    {
        var path = Guid.NewGuid().ToString();
        
        var fileData = new FileData(command.Stream, command.BucketName, path);
        
        var result = await _fileProvider.Upload(fileData, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        _logger.LogInformation("Uploaded file with path {path} in bucket {bucket}", path, fileData.BucketName);

        return result.Value;
    }
}