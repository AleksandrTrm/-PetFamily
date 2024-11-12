using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.FileProviders;
using FileInfo = PetFamily.Shared.SharedKernel.FileProviders.FileInfo;

namespace PetFamily.Shared.SharedKernel.Abstractions;

public interface IFileProvider
{
    Task<Result<IEnumerable<string>, Error.Error>> UploadFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error.Error>> Remove(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error.Error>> GetFile(
        FileContent fileContent,
        CancellationToken cancellationToken = default);

    Task<Result<List<string>, Error.Error>> GetFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);
}