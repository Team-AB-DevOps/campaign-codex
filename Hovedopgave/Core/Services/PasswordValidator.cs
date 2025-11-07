
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

        if (!password.Any(char.IsUpper))
        {
            return false;
        }
        
        if (!password.Any(char.IsLower))
        {
            return false;
        }
        
        if (!password.Any(char.IsDigit))
        {
            return false;
        }
        
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            return false;
        }

        return true;
    }
}