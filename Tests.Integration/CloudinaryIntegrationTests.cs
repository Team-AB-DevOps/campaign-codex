using FluentAssertions;
using Hovedopgave.Core.Configuration;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Photos.DTOs;
using Microsoft.Extensions.Options;
using Tests.Integration.Infrastructure;

namespace Tests.Integration;


[Trait("test", "cloudinary")]
public class CloudinaryUploadTests : IAsyncLifetime
{
    private string _cachedId = string.Empty;
    private readonly CloudinaryService _cloudinaryService;
    
    public CloudinaryUploadTests()
    {
        IOptions<CloudinarySettings> config = Options.Create(new CloudinarySettings
        {
            CloudName = "dkcqshlp4",
            ApiKey = "426284213378781",
            ApiSecret = "WRaHhyJ4ta2kFKWKHjOrWWba4OA"
        });
        
        _cloudinaryService = new CloudinaryService(config);
    }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
    
    public async Task DisposeAsync()
    {
        await _cloudinaryService.DeletePhoto(_cachedId);
    }
    
    [Theory]
    [InlineData("png")]
    [InlineData("jpg")]
    [InlineData("webp")]
    [InlineData("avif")]
    public async Task UploadPhoto_ShouldReturnPhotoUploadResultOnPng(string dataType)
    {
        // Arrange
        var file = CloudinaryHelper.CreateTestFormFile(dataType);
        
        // Act
        var result = await _cloudinaryService.UploadPhoto(file);

        // Asert
        Assert.NotNull(result);
        result.Should().BeOfType<PhotoUploadResult>();
        result.PublicId.Should().NotBeNullOrEmpty();
        result.Url.Should().NotBeNullOrEmpty();
        _cachedId = result.PublicId;
    }
}

[Trait("test", "cloudinary")]
public class CloudinaryDeleteTests
{
    private readonly CloudinaryService _cloudinaryService;
    
    public CloudinaryDeleteTests()
    {
        IOptions<CloudinarySettings> config = Options.Create(new CloudinarySettings
        {
            CloudName = "dkcqshlp4",
            ApiKey = "426284213378781",
            ApiSecret = "WRaHhyJ4ta2kFKWKHjOrWWba4OA"
        });
        
        _cloudinaryService = new CloudinaryService(config);
    }
    
    [Fact]
    public async Task DeletePhoto_ShouldBeSuccessfulOnExistingId()
    {
        // Arrange
        var file = CloudinaryHelper.CreateTestFormFile();
        var response = await _cloudinaryService.UploadPhoto(file);
        
        // Act
        var result = await _cloudinaryService.DeletePhoto(response!.PublicId);
    
        // Asert
        result.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task DeletePhoto_ShouldHandleNonExistingPhotoGracefully()
    {
        // Arrange
        var randomId = Random.Shared.Next(100000, 999999).ToString();
    
        // Act
        var act = async () => await _cloudinaryService.DeletePhoto(randomId);
    
        // Asert
        await act.Should().NotThrowAsync<Exception>();
    }
}
