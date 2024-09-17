using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Files.Delete;
using PetFamily.Application.Volunteers.Files.Get.GetFile;
using PetFamily.Application.Volunteers.Files.Get.GetFiles;
using PetFamily.Application.Volunteers.Files.Upload;

namespace PetFamily.API.Controllers.Files;

public class FileController : ApplicationController
{
    private const string BUCKET_NAME = "photos";

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        
        var request = new UploadFileCommand(stream, BUCKET_NAME);

        var uploadFileResult = await handler.Handle(request, cancellationToken);
        if (uploadFileResult.IsFailure)
            return uploadFileResult.Error.ToResponse();
        
        return Ok(uploadFileResult.Value);
    }

    [HttpDelete("{path:guid}")]
    public async Task<IActionResult> RemoveFile(
        [FromRoute] Guid path,
        [FromServices] RemoveFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new RemoveFileCommand(BUCKET_NAME, path.ToString());

        var removeFileResult = await handler.Handle(request, cancellationToken);
        if (removeFileResult.IsFailure)
            return removeFileResult.Error.ToResponse();

        return Ok(removeFileResult.Value);
    }

    [HttpGet("{path:guid}")]
    public async Task<IActionResult> GetFile(
        [FromRoute] Guid path,
        [FromServices] GetFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFileCommand(BUCKET_NAME, path.ToString());

        var getFileResult = await handler.Handle(request, cancellationToken);
        if (getFileResult.IsFailure)
            return getFileResult.Error.ToResponse();

        return Ok(getFileResult.Value);
    }
    
    [HttpGet("photos")]
    public async Task<IActionResult> GetFiles(
        [FromBody] IEnumerable<GetFileCommand> filesToGet,
        [FromServices] GetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFilesCommand(filesToGet);

        var getFilesResult = await handler.Handle(request, cancellationToken);
        if (getFilesResult.IsFailure)
            return getFilesResult.Error.ToResponse();

        return Ok(getFilesResult.Value);
    }
} 