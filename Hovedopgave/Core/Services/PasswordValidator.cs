
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hovedopgave.Core.Services;

public static class PasswordValidator
{
    private const int MinLength = 8;
    private const int MaxLength = 50;

    public static bool IsValidate(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < MinLength || password.Length > MaxLength)
        {
            return false;
        }

        return password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(c => !char.IsLetterOrDigit(c));
    }
    
    // Check if password has been pwned using the Have I Been Pwned API
    // Works by sending the first 5 characters of the SHA-1 hash of the password
    // and checking if the rest of the hash is in the response
    public static async Task<bool> IsPwned(string password)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.pwnedpasswords.com/")
        };
        var hashString = ComputeSha1Hash(password);
        var prefix = hashString.Substring(0, 5);
        var suffix = hashString.Substring(5);
        var response = await httpClient.GetAsync($"range/{prefix}");
        
        if (!response.IsSuccessStatusCode)
        {
            throw new BadHttpRequestException($"Error checking password against Pwned Passwords API, {response.StatusCode}, {response.ReasonPhrase}, {await response.Content.ReadAsStringAsync()}");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        var hashes = content.Split('\n').Select(l => l.Split(":")[0].Trim()).ToHashSet();
        
        return hashes.Any(line => line.StartsWith(suffix, StringComparison.OrdinalIgnoreCase));
    }
    
    private static string ComputeSha1Hash(string input)
    {
        using var sha1 = System.Security.Cryptography.SHA1.Create();
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hashBytes = sha1.ComputeHash(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
    }
}