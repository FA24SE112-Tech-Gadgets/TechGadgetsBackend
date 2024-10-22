using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Common.Settings;

namespace WebApi.Features.Images;

[ApiController]
public class UploadImage(IOptions<GoogleStorageSettings> googleStorageSettings) : ControllerBase
{
    private readonly GoogleStorageSettings _googleStorageSettings = googleStorageSettings.Value;
    private readonly StorageClient _storageClient = StorageClient.Create();

    public new class Request
    {
        public IFormFile File { get; set; } = default!;
    }

    [HttpPost("upload-images")]
    [Tags("Upload Images")]
    public async Task<IActionResult> Handler(Request request)
    {
        var file = request.File;
        var fileName = Guid.NewGuid().ToString();

        if (file is null)
        {
            return BadRequest("File must not null");
        }

        string folderAndFileName = $"Gadgets/{fileName}.{Path.GetExtension(file.FileName)[1..]}";

        await _storageClient.UploadObjectAsync(
                   _googleStorageSettings.Bucket,
                   folderAndFileName,
                   file.ContentType,
                   file.OpenReadStream());

        string publicUrl = $"https://storage.googleapis.com/{_googleStorageSettings.Bucket}/{folderAndFileName}";

        return Ok(publicUrl);
    }
}
