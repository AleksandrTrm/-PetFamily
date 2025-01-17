﻿using Microsoft.Extensions.Logging;
using PetFamily.VolunteersManagement.Application.BackgroundServices;
using PetFamily.VolunteersManagement.Application.FileProvider;
using PetFamily.VolunteersManagement.Application.Messaging;
using FileInfo = PetFamily.VolunteersManagement.Application.FileProvider.FileInfo;


namespace PetFamily.VolunteersManagement.Infrastructure.Services;

public class FilesCleanerService : IFilesCleanerService
{
    private IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private IFileProvider _fileProvider;
    private ILogger<FilesCleanerService> _logger;

    public FilesCleanerService(
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<FilesCleanerService> logger,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _fileProvider = fileProvider;
        _messageQueue = messageQueue;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var files = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var file in files)
            await _fileProvider.Remove(file, cancellationToken);
        
        _logger.LogInformation("Uncreated pet files has been deleted");
    }
}