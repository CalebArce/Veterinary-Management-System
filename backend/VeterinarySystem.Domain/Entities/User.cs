using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Domain.Entities;

public class User : AuditableEntity
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Assistant;

    public bool IsActive { get; set; } = true;
}