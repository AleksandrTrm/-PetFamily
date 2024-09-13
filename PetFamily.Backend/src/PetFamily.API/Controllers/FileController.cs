using Minio;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Files.Delete;
using PetFamily.Application.Volunteers.Files.UploadFile;
using PetFamily.Application.Volunteers.Files.Get.GetFile;
using PetFamily.Application.Volunteers.Files.Get.GetFiles;

namespace PetFamily.API.Controllers;

public class FileController : ApplicationController
{
    private const string BUCKET_NAME = "photos";

    public FileController()
    {
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        
        var request = new UploadFileRequest(stream, BUCKET_NAME);

        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpDelete("{path:guid}")]
    public async Task<IActionResult> RemoveFile(
        [FromRoute] Guid path,
        [FromServices] RemoveFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new RemoveFileRequest(BUCKET_NAME, path.ToString());

        var result = await handler.Handle(request, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{path:guid}")]
    public async Task<IActionResult> GetFile(
        [FromRoute] Guid path,
        [FromServices] GetFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFileRequest(BUCKET_NAME, path.ToString());

        var result = await handler.Handle(request, cancellationToken);

        return Ok(result.Value);
    }
    
    [HttpGet("photos")]
    public async Task<IActionResult> GetFiles(
        [FromBody] IEnumerable<GetFileRequest> filesToGet,
        [FromServices] GetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFilesRequest(filesToGet);

        var result = await handler.Handle(request, cancellationToken);

        return Ok(result.Value);
    }
} 