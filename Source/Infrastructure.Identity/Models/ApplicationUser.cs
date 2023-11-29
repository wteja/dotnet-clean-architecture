using Application.DTOs.Account;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public bool OwnRefreshToken(string token)
    {
        return RefreshTokens.Any(it => it.Token == token);
    }
}
