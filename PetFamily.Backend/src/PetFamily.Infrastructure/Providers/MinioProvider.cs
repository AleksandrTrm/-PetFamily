using Minio;
using Minio.DataModel.Args;
using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Files.Get.GetFile;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private IMinioClient _minioClient;
    private ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _logger = logger;
        _minioClient = minioClient;
    }

    public async Task<Result<string, Error>> Upload(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck(fileData.BucketName, cancellationToken);

            var path = Guid.NewGuid();

            var fileArgs = new PutObjectArgs()
                .WithObject(path.ToString())
                .WithBucket(fileData.BucketName)
                .WithObjectSize(fileData.Stream.Length)
                .WithStreamData(fileData.Stream);

            var result = await _minioClient.PutObjectAsync(fileArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }

    public async Task<Result<string, Error>> Remove(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck(fileData.BucketName, cancellationToken);

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.Path);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileData.Path;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete file in minio");
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result<string, Error>> GetFile(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistsCheck(fileData.BucketName, cancellationToken);

            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.Path)
                .WithExpiry(60 * 60);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File on the path '{path}' does not exists", fileData.Path);
            return Error.Failure("file.get", $"The file can not be found at the path '{fileData.Path}'");
        }
    }

    public async Task<Result<List<string>, Error>> GetFiles(
        IEnumerable<FileData> filesData, 
        CancellationToken cancellationToken = default)
    {
        List<string> filesLinks = [];
        foreach (var fileData in filesData)
        {
            try
            {
                await BucketExistsCheck(fileData.BucketName, cancellationToken);

                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithObject(fileData.Path)
                    .WithExpiry(60 * 60);

                filesLinks.Add(await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File on the path '{path}' does not exists", fileData.Path);
                return Error.Failure("file.get", $"The file can not be found at the path '{fileData.Path}'");
            }
        }

        return filesLinks;
    }

    private async Task BucketExistsCheck(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        var bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        if (bucketExists == false)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
    }
}