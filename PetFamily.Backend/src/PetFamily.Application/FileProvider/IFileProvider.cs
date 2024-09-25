using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
    Task<Result<IEnumerable<string>, Error>> UploadFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> Remove(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetFile(
        FileContent fileContent,
        CancellationToken cancellationToken = default);

    Task<Result<List<string>, Error>> GetFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);
}