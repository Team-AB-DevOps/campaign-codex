using Microsoft.AspNetCore.Http;

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
            Headers = new HeaderDictionary()
        };
    }
}