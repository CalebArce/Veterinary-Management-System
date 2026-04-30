using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;

namespace VeterinarySystem.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration configuration;

    public JwtTokenService(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public string GenerateToken(User user, out DateTime expiresAt)
    {
        var jwtSection = configuration.GetSection("Jwt");

        var key = jwtSection["Key"]!;
        var issuer = jwtSection["Issuer"]!;
        var audience = jwtSection["Audience"]!;
        var expiresInMinutes = int.Parse(jwtSection["ExpiresInMinutes"]!);

        expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new List<Claim>
        {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.FullName),
          new Claim(ClaimTypes.Email, user.Email),
          new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}