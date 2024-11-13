using CSharpFunctionalExtensions;

namespace PetFamily.VolunteersManagement.Application.FileProvider;

public interface IFileProvider
{
    Task<Result<IEnumerable<string>, Shared.SharedKernel.Error.Error>> UploadFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);

    Task<Result<string, Shared.SharedKernel.Error.Error>> Remove(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);

    Task<Result<string, Shared.SharedKernel.Error.Error>> GetFile(
        FileContent fileContent,
        CancellationToken cancellationToken = default);

    Task<Result<List<string>, Shared.SharedKernel.Error.Error>> GetFiles(
        IEnumerable<FileContent> files,
        CancellationToken cancellationToken = default);
}