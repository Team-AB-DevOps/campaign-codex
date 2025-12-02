using Hovedopgave.Core.Controllers;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Account.DTOs;
using Hovedopgave.Features.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Hovedopgave.Features.Account;

public class AccountController(SignInManager<User> signInManager) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
    {

        if (!PasswordValidator.IsValidate(registerDto.Password))
        {
            ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            return ValidationProblem();
        }

        if (await PasswordValidator.IsPwned(registerDto.Password))
        {
            ModelState.AddModelError("Password", "Password is Pwned. Pick a more secure password.");
            return ValidationProblem();
        }

        var user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return Ok();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem();
    }

    [AllowAnonymous]
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        // Debug logging for cookies
        Log.Information("=== USER-INFO REQUEST DEBUG ===");
        Log.Information("Cookies received: {CookieCount}", Request.Cookies.Count);
        foreach (var cookie in Request.Cookies)
        {
            Log.Information("Cookie: {Key} = {Value}", cookie.Key, cookie.Value[..Math.Min(50, cookie.Value.Length)] + "...");
        }
        Log.Information("IsAuthenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
        Log.Information("Authorization Header: {AuthHeader}", Request.Headers["Authorization"].ToString());
        Log.Information("=== END DEBUG ===");

        if (User.Identity?.IsAuthenticated == false)
        {
            return NoContent();
        }

        var user = await signInManager.UserManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            user.DisplayName,
            user.Email,
            user.Id,
        });
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }
}
