using VeterinarySystem.Domain.Entities;

namespace VeterinarySystem.Application.Interfaces;

public interface IRefreshTokenService
{
    string GenerateRefreshToken();

    Task<RefreshToken> CreateAsync(User user);

    Task<RefreshToken> ValidateAsync(string refreshToken);

    Task RevokeAsync(string refreshToken);
}