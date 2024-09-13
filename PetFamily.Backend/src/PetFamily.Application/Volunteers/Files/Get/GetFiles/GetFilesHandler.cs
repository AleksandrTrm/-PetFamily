using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Files.Get.GetFiles;

public class GetFilesHandler
{
    private ILogger<GetFilesHandler> _logger;
    private IFileProvider _fileProvider;

    public GetFilesHandler(IFileProvider fileProvider, ILogger<GetFilesHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<List<string>, Error>> Handle(
        GetFilesRequest request, 
        CancellationToken cancellationToken = default)
    {
        List<FileData> filesData = [];   
        foreach (var getFileRequest in request.GetFilesRequests)
        {
            var fileData = new FileData(null, getFileRequest.BucketName, getFileRequest.Path); 
            
            filesData.Add(fileData);
        }

        var result = await _fileProvider.GetFiles(filesData, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result;
    }
}