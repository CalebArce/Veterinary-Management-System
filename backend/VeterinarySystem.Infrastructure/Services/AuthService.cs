using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.Auth;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;
using VeterinarySystem.Domain.Enums;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext context;
    private readonly IJwtTokenService jwtTokenService;
    private readonly IAuditLogService auditLogService;
    private readonly IRefreshTokenService refreshTokenService;

    public AuthService(AppDbContext _context, IJwtTokenService _jwtTokenService,
                        IAuditLogService _auditLogService, IRefreshTokenService _refreshTokenService)
    {
        context = _context;
        jwtTokenService = _jwtTokenService;
        auditLogService = _auditLogService;
        refreshTokenService = _refreshTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
    {
        var exists = await context.Users.AnyAsync(u => u.Email == dto.Email);

        if (exists)
            throw new Exception("Ya existe un ususario con ese correo");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = dto.Role,
            IsActive = true
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        await auditLogService.RegisterAsync(
            AuditAction.RegisterUser,
            "User",
            user.Id,
            $"Usuario registrado: {user.Email}");

        var token = jwtTokenService.GenerateToken(user, out var expiresAt);
        var refreshToken = await refreshTokenService.CreateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token,
            ExpiresAt = expiresAt,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);
        
        if (user == null)
            throw new Exception("Credenciales inválidas");
        

        var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        Console.WriteLine("Resultado verify: " + validPassword);

        if (!validPassword)
            throw new Exception("Credenciales inválidas");

        var token = jwtTokenService.GenerateToken(user, out var expiresAt);
        var refreshToken = await refreshTokenService.CreateAsync(user);

        await auditLogService.RegisterAsync(
            AuditAction.Login,
            "User",
            user.Id,
            $"Inicio de sesión exitoso: {user.Email}");

        return new AuthResponseDto
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token,
            ExpiresAt = expiresAt,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshTokenRequestDto dto)
    {
        var storedToken = await refreshTokenService.ValidateAsync(dto.RefreshToken);

        await refreshTokenService.RevokeAsync(dto.RefreshToken);

        var accessToken = jwtTokenService.GenerateToken(storedToken.User, out var expiresAt);
        var newRefreshToken = await refreshTokenService.CreateAsync(storedToken.User);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = expiresAt,
            FullName = storedToken.User.FullName,
            Email = storedToken.User.Email,
            Role = storedToken.User.Role.ToString()
        };
    }

    public async Task LogoutAsync(LogoutRequestDto dto)
    {
        await refreshTokenService.RevokeAsync(dto.RefreshToken);
    }
}