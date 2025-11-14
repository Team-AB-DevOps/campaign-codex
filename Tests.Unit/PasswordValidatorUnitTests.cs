using FluentAssertions;
using Hovedopgave.Core.Services;

namespace Tests.Unit;

public class PasswordValidatorUnitTests
{
    [Theory]
    [InlineData("Password1!")]
    [InlineData("Pass123!")]
    [InlineData("IAmExactlyAtMaxLength1!!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    [InlineData("IAmExactlyAtMaxLength1!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    [InlineData("Pass123!@#Word")]
    [InlineData("!Password1!")]
    public void IsValidate_Should_ReturnTrue_When_PasswordIsValid(string password)
    {
        // Arrange & Act
        var result = PasswordValidator.IsValidate(password);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Ps1!")]
    [InlineData("Pas12!A")]
    [InlineData("IAmExactlyTooLong1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    [InlineData("IAmWayTooLong1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    [InlineData("password1!")]
    [InlineData("Password!")]
    [InlineData("Password1")]
    [InlineData("passwordonly")]
    [InlineData("PASSWORDONLY")]
    [InlineData("12345678")]
    [InlineData("!@#$%^&*")]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    public void IsValidate_Should_ReturnFalse_When_PasswordIsInvalid(string? password)
    {
        // Arrange & Act
        var result = PasswordValidator.IsValidate(password);

        // Assert
        result.Should().BeFalse();
    }
}