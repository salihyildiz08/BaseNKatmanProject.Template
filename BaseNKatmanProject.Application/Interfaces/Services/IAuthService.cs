using BaseNKatmanProject.Application.DTOs.Auth;

namespace BaseNKatmanProject.Application.Interfaces.Services;
public interface IAuthService
{
    Task<AuthDto> RegisterAsync(RegisterDto request);
    Task<AuthDto> LoginAsync(LoginDto request);
}