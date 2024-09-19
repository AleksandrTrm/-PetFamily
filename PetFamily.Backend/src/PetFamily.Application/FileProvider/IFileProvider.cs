using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public interface IFileProvider
{
    Task<Result<IEnumerable<string>, Error>> UploadFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> Remove(
        FileContent fileContent,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetFile(
        FileContent fileContent,
        CancellationToken cancellationToken = default);

    Task<Result<List<string>, Error>> GetFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);
}