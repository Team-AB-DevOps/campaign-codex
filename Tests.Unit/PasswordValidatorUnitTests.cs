using FluentAssertions;
using Hovedopgave.Core.Services;

namespace Tests.Unit;

public class PasswordValidatorUnitTests
{
    #region Length Validation Tests

    [Theory]
    [InlineData("Abcdef1!")]
    [InlineData("Abcdefg2!")]
    [InlineData("Val1dPassword!Sample1")]
    [InlineData("Password1!Password1!Password1!Password1!Secure1!X")]
    [InlineData("Password1!Password1!Password1!Password1!Password1!")]
    public void IsValidate_Should_ReturnTrue_When_PasswordLengthIsValid(string password)
    {
        // Act
        var result = PasswordValidator.IsValidate(password);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Ab!")]
    [InlineData("Abc1!xY")]
    [InlineData("Password1!Password1!Password1!Password1!Password1!P")]
    [InlineData("Password1!Password1!Password1!Password1!Password1!Password1!")]
    public void IsValidate_Should_ReturnFalse_When_PasswordLengthIsInvalid(string? password)
    {
        // Act
        var result = PasswordValidator.IsValidate(password);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Character Requirements Tests

    [Theory]
    [InlineData("Abcdef1!!")]
    [InlineData("Abcd ef1!")]
    [InlineData("ÆbleGrød1!")]
    public void IsValidate_Should_ReturnTrue_When_PasswordHasAllRequiredCharacters(string password)
    {
        // Act
        var result = PasswordValidator.IsValidate(password);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("password1!")]
    [InlineData("Password!")]
    [InlineData("Password1")]
    [InlineData("password")]
    [InlineData("password1")]
    [InlineData("Password")]
    [InlineData(" Abcdef1!")]
    [InlineData("Abcdef1! ")]
    [InlineData("ABCD123!")]
    public void IsValidate_Should_ReturnFalse_When_PasswordMissingRequiredCharacters(
        string password
    )
    {
        // Act
        var result = PasswordValidator.IsValidate(password);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
