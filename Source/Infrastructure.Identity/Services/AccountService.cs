using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.Account;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Seetings;
using Infrastructure.Identity.Helpers;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ApiException(StatusCodes.Status404NotFound, $"No accounts registered with {request.Email}.");
        }

        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ApiException($"This user has empty username.");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
        if (!result.Succeeded)
        {
            throw new ApiException(StatusCodes.Status401Unauthorized, $"Invalid credential for ${request.Email}.");
        }

        if (!user.EmailConfirmed)
        {
            throw new ApiException(StatusCodes.Status401Unauthorized, $"Account not confirmed for ${request.Email}.");
        }
        JwtSecurityToken token = await GenerateJWToken(user);
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        var response = new AuthenticationResponse
        {
            JWToken = new JwtSecurityTokenHandler().WriteToken(token),
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email!,
            IsVerified = user.EmailConfirmed,
            Roles = rolesList.ToList(),
            RefreshToken = GenerateRefreshToken(ipAddress).Token,
        };
        return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}.");
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        string ipAddress = IpHelper.GetIpAddress();

        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id.ToString()),
            new Claim("ip", ipAddress),
        }.Union(userClaims).Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtToken;
    }

    public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ApiException(StatusCodes.Status404NotFound, $"No accounts ID: {userId}.");
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded) throw new ApiException($"An error occured while confirming {user.Email}.");

        return new Response<string>(user.Id, message: $"Account confirmed for {user.Email}.");
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        // always return ok response to prevent email enumeration
        if (user == null) return;

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        // Sending email to user.
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var existUser = await _userManager.FindByNameAsync(request.UserName);
        if (existUser != null)
        {
            throw new ApiException($"Username '${request.UserName}' is already taken.");
        }

        var existEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existEmail != null)
        {
            throw new ApiException($"Email '${request.Email}' is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any())
            {
                throw new ValidationException(result.Errors.Select(it => it.Description));
            }
            else
            {
                throw new ApiException($"Error occured while registering with {request.Email}.");
            }
        }

        var verifyUrl = string.Empty;
        return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL: {verifyUrl}");
    }

    public async Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new ApiException(StatusCodes.Status404NotFound, $"No account registered with {request.Email}.");
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded) throw new ApiException($"Error occored while resetting the password.");

        return new Response<string>(request.Email, message: $"Password resetted.");
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress,
        };
    }

    private static string RandomTokenString()
    {
        var randomBytes = new byte[40];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}