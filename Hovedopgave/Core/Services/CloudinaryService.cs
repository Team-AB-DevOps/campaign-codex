using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hovedopgave.Core.Configuration;
using Hovedopgave.Core.Interfaces;
using Hovedopgave.Features.Photos.DTOs;
using Microsoft.Extensions.Options;

namespace Hovedopgave.Core.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            Environment.GetEnvironmentVariable("CloudName") ?? throw new InvalidOperationException("CloudName not set"),
            Environment.GetEnvironmentVariable("ApiKey") ?? throw new InvalidOperationException("ApiKey not set"),
            Environment.GetEnvironmentVariable("ApiSecret") ?? throw new InvalidOperationException("ApiSecret not set")
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> DeletePhoto(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error != null)
        {
            throw new Exception(result.Error.Message);
        }

        return result.Result;
    }

    public async Task<PhotoUploadResult?> UploadPhoto(IFormFile file)
    {
        if (file.Length <= 0)
        {
            return null;
        }

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            // Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            Folder = "Hovedopgave"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        return new PhotoUploadResult
        {
            PublicId = uploadResult.PublicId,
            Url = uploadResult.SecureUrl.AbsoluteUri
        };
    }
}
