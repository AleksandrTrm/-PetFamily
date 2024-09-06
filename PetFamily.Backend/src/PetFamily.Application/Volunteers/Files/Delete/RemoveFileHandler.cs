using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Files.Delete;

public class RemoveFileHandler
{
    private ILogger<RemoveFileHandler> _logger;
    private IFileProvider _fileProvider;

    public RemoveFileHandler(IFileProvider fileProvider, ILogger<RemoveFileHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(
        RemoveFileRequest request, 
        CancellationToken cancellationToken = default)
    {
        var fileData = new FileData(null, request.BucketName, request.Path);
        
        var result = await _fileProvider.Remove(fileData, cancellationToken);
        
        _logger.LogInformation("File with path '{path}' has been deleted from bucket '{bucket}'", 
            fileData.Path, fileData.BucketName);
        
        return result.Value;
    } 
}