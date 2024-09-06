﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Files.UploadFile;

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
        UploadFileRequest request, 
        CancellationToken cancellationToken = default)
    {
        var path = Guid.NewGuid().ToString();
        
        var fileData = new FileData(request.Stream, request.BucketName, path);
        
        var result = await _fileProvider.Upload(fileData, cancellationToken);
        
        _logger.LogInformation("Uploaded file with path {path} in bucket {bucket}", path, fileData.BucketName);

        return result.Value;
    }
}