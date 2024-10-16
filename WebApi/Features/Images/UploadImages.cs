using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Common.Settings;

namespace WebApi.Features.Images;

[ApiController]
public class UploadImagesController(IOptions<GoogleStorageSettings> googleStorageSettings) : ControllerBase
{
    private readonly GoogleStorageSettings _googleStorageSettings = googleStorageSettings.Value;
    private readonly StorageClient _storageClient = StorageClient.Create();

    public new class Request
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }

    [HttpPost("upload-images/multiple")]
    [Tags("Upload Images")]
    public async Task<IActionResult> Handler([FromForm] Request request)
    {
        if (request.Files == null || request.Files.Count == 0)
        {
            return BadRequest("File must not be null or empty");
        }

        var publicUrls = new List<string>();

        foreach (var file in request.Files)
        {
            var fileName = Guid.NewGuid().ToString();
            string folderAndFileName = $"Gadgets/{fileName}{Path.GetExtension(file.FileName)}";

            using (var stream = file.OpenReadStream())
            {
                await _storageClient.UploadObjectAsync(
                    _googleStorageSettings.Bucket,
                    folderAndFileName,
                    file.ContentType,
                    stream);
            }

            string publicUrl = $"https://storage.googleapis.com/{_googleStorageSettings.Bucket}/{folderAndFileName}";
            publicUrls.Add(publicUrl);
        }

        return Ok(publicUrls);
    }
}

