using Minio;
using Minio.DataModel.Args;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.Exceptions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared.Error;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
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
            await BucketExistsCheck(fileContents.Select(f => f.FileInfo.BucketName), cancellationToken);
            
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
            .WithBucket(fileContent.FileInfo.BucketName)
            .WithStreamData(fileContent.Stream)
            .WithObjectSize(fileContent.Stream.Length)
            .WithObject(fileContent.FileInfo.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileContent.FileInfo.Path;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while uploading file with path - '{path}' to bucket '{bucket}'",
                fileContent.FileInfo.Path, fileContent.FileInfo.BucketName);

            return Error.Failure("File.Upload", "Occurred error while uploading file to minio");
        }
        finally
        {
            semaphoreSlim.Release();
        } 
    }

    public async Task<Result<string, Error>> Remove(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck([fileInfo.BucketName], cancellationToken);

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.Path);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileInfo.Path;
        }
        catch (AuthorizationException ex)
        {
            _logger.LogError(ex, "Authorization failed");

            return Error
                .Failure("authorization.failed", "Authorization failed while connection to file provider");
        }
        catch (InvalidBucketNameException ex)
        {
            _logger.LogError(ex, "Invalid bucket name while trying to delete file");

            return Error
                .Failure("invalid.bucket.name", "Failed to delete file because bucket name was invalid");
        }
        catch (InvalidObjectNameException ex)
        {
            _logger.LogError(ex, "Invalid object name");

            return Error
                .Failure("invalid.object.name", "Object name was invalid");
        }
        catch (BucketNotFoundException ex)
        {
            _logger.LogError(ex, "Bucket not found");

            return Error
                .Failure("bucket.not.found", $"Can not find bucket with name '{fileInfo.BucketName}'");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete file in minio");
            
            return Error
                .Failure("file.delete", $"Fail to delete file in minio with path - '{fileInfo.Path}'");
        }
    }

    public async Task<Result<string, Error>> GetFile(
        FileContent fileContent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck([fileContent.FileInfo.BucketName], cancellationToken);

            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileContent.FileInfo.BucketName)
                .WithObject(fileContent.FileInfo.Path)
                .WithExpiry(60 * 60);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File on the path '{path}' does not exists", fileContent.FileInfo.Path);
            
            return Error.Failure(
                "file.get",
                $"The file can not be found at the path '{fileContent.FileInfo.Path}'");
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
                await BucketExistsCheck([fileContent.FileInfo.BucketName], cancellationToken);

                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileContent.FileInfo.BucketName)
                    .WithObject(fileContent.FileInfo.Path)
                    .WithExpiry(60 * 60);

                filesLinks.Add(await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File on the path '{path}' does not exists", fileContent.FileInfo.Path);
                
                return Error.Failure(
                    "file.get", 
                    $"The file can not be found at the path '{fileContent.FileInfo.Path}'");
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