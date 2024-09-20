using PetFamily.Application.Volunteers.Pet.UploadPetFiles;

namespace PetFamily.API.Processors;

public class FileProcessor : IAsyncDisposable
{
    private readonly List<UploadFileCommand> _files = [];

    public List<UploadFileCommand> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileContent = new UploadFileCommand(stream, file.FileName);

            _files.Add(fileContent);
        }

        return _files;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var file in _files)
        {
            await file.Content.DisposeAsync();            
        }
    }
}