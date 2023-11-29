using Application.DTOs.Account;
using Application.Wrappers;

namespace Application.Interfaces;

public interface IAccountService
{
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
    Task<Response<string>> RegisterAsync(RegisterRequest request);
    Task<Response<string>> ConfirmEmailAsync(string userId, string code);

    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest request);
}