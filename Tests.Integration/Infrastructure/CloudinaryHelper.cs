using DotNetEnv;
using Hovedopgave.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Tests.Integration.Infrastructure;

public static class CloudinaryHelper
{
    public static IFormFile CreateTestFormFile(string type = "jpg")
    {
        var filePath = Path.Combine("mocks", $"picture.{type}");
        var fileName = Path.GetFileName(filePath);
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
        };
    }

    public static IOptions<CloudinarySettings> CreateCloudinarySettingsFile()
    {
        Env.TraversePath().Load();
        
        return Options.Create(
            new CloudinarySettings
            {
                CloudName = Environment.GetEnvironmentVariable("CloudName") ?? throw new InvalidOperationException("CloudName not set"),
                ApiKey = Environment.GetEnvironmentVariable("ApiKey") ?? throw new InvalidOperationException("ApiKey not set"),
                ApiSecret = Environment.GetEnvironmentVariable("ApiSecret") ?? throw new InvalidOperationException("ApiSecret not set"),
            }
        );
    }
}
