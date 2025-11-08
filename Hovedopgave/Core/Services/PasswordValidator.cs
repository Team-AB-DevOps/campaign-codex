
namespace Hovedopgave.Core.Services;

public static class PasswordValidator
{
    private const int MinLength = 8;

    public static bool IsValidate(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < MinLength)
        {
            return false;
        }

        return password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(c => !char.IsLetterOrDigit(c));
    }
}