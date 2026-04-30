using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext context;

    public RefreshTokenService(AppDbContext _context)
    {
        context = _context;
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<RefreshToken> CreateAsync(User user)
    {
        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<RefreshToken> ValidateAsync(string refreshToken)
    {
        var token = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token == null)
            throw new Exception("Refresh token inválido");

        if (token.IsRevoked)
            throw new Exception("Refresh token revocado");

        if (token.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Refresh token expirado");

        if (!token.User.IsActive)
            throw new Exception("El usuario está inactivo");

        return token;
    }

    public async Task RevokeAsync(string refreshToken)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token == null)
            throw new Exception("Refresh token inválido");

        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}