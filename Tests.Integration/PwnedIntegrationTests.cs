using Hovedopgave.Core.Services;

namespace Tests.Integration;

public class PwnedIntegrationTests
{
    private static string GenerateRandomString(int length = 30)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("qwerty")]
    [InlineData("password")]
    [InlineData("letmein")]
    [InlineData("")] // Edge-case
    [InlineData(" ")] // Edge-case
    public async Task PwnedPassword_ShouldReturnTrue(string password)
    {
        // Arrange and Act
        var isPwned = await PasswordValidator.IsPwned(password);

        // Assert
        Assert.True(isPwned);
    }
    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public async Task PwnedPassword_ShouldReturnFalse(int _) // Repeat 10 times
    {
        // Arrange
        var password = GenerateRandomString();
        
        // Act
        var isPwned = await PasswordValidator.IsPwned(password);

        // Assert
        Assert.False(isPwned);
    }
    
}
