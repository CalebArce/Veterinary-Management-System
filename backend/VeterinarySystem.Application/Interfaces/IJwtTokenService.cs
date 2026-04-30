using VeterinarySystem.Domain.Entities;

namespace VeterinarySystem.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user, out DateTime expiresAt);
}