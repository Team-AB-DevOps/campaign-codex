using System.Security.Claims;
using Hovedopgave.Core.Data;
using Hovedopgave.Features.Account.Models;
using Microsoft.AspNetCore.Identity;

namespace Hovedopgave.Core.Services;

public class UserAccessor(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext) : IUserAccessor
{
    public async Task<User> GetUserAsync()
    {
        return await dbContext.Users.FindAsync(GetUserId())
               ?? throw new UnauthorizedAccessException("No user logged in");
    }


    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? throw new Exception("No user found");
    }


}
