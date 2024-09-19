using Minio;
using Minio.DataModel.Args;
using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MIN_FILES_COUNT = 1;
    private const int THREADS_LIMIT = 5;
    
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _logger = logger;
        _minioClient = minioClient;
    }

    public async Task<Result<IEnumerable<string>, Error>> UploadFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default)
    {
        var fileContents = files.ToList();
        
        var semaphoreSlim = new SemaphoreSlim(THREADS_LIMIT);
        try
        {
            await BucketExistsCheck(fileContents.Select(f => f.BucketName), cancellationToken);
            
            var tasks = fileContents
                .Select(async f => await UploadFile(f, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var paths = pathsResult.Select(p => p.Value).ToList();

            return paths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload files in minio");
            return Error.Failure("files.upload.error", "Failed to upload files to storage");
        }
    }

    private async Task<Result<string, Error>> UploadFile(
        FileContent fileContent,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileContent.BucketName)
            .WithStreamData(fileContent.Stream)
            .WithObjectSize(fileContent.Stream.Length)
            .WithObject(fileContent.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileContent.Path.Path;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while uploading file with path - '{path}' to bucket '{bucket}'",
                fileContent.Path, fileContent.BucketName);

            return Error.Failure("File.Upload", "Occurred error while uploading file to minio");
        }
        finally
        {
            semaphoreSlim.Release();
        } 
    }

    public async Task<Result<string, Error>> Remove(
        FileContent fileContent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck([fileContent.BucketName], cancellationToken);

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileContent.BucketName)
                .WithObject(fileContent.Path);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileContent.Path.Path;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete file in minio");
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result<string, Error>> GetFile(
        FileContent fileContent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck([fileContent.BucketName], cancellationToken);

            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileContent.BucketName)
                .WithObject(fileContent.Path)
                .WithExpiry(60 * 60);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File on the path '{path}' does not exists", fileContent.Path);
            return Error.Failure("file.get", $"The file can not be found at the path '{fileContent.Path}'");
        }
    }

    public async Task<Result<List<string>, Error>> GetFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default)
    {
        List<string> filesLinks = [];
        foreach (var fileContent in files)
        {
            try
            {
                await BucketExistsCheck([fileContent.BucketName], cancellationToken);

                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileContent.BucketName)
                    .WithObject(fileContent.Path)
                    .WithExpiry(60 * 60);

                filesLinks.Add(await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File on the path '{path}' does not exists", fileContent.Path);
                return Error.Failure("file.get", $"The file can not be found at the path '{fileContent.Path}'");
            }
        }

        return filesLinks;
    }

    private async Task BucketExistsCheck(
        IEnumerable<string> bucketsNames,
        CancellationToken cancellationToken = default)
    {
        var bucketsNamesList = bucketsNames.ToList();

        foreach (var bucketName in bucketsNamesList)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist is false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}