using VeterinarySystem.Application.DTOs.Auth;

namespace VeterinarySystem.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);

    Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto);

    Task<AuthResponseDto> RefreshAsync(RefreshTokenRequestDto dto);

    Task LogoutAsync(LogoutRequestDto dto);
}