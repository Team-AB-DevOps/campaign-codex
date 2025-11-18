using System.ComponentModel.DataAnnotations;

namespace Hovedopgave.Features.Account.DTOs;

public record RegisterDto
{
    [Required] public string DisplayName { get; init; } = "";
    [Required][EmailAddress] public string Email { get; init; } = "";
    public string Password { get; init; } = "";
}