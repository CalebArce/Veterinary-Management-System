using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Application.DTOs.AuditLogs;

public class AuditLogResponseDto
{
    public int Id { get; set; }

    public AuditAction Action { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public int? EntityId { get; set; }

    public string? Description { get; set; }

    public string? UserEmail { get; set; }

    public string? UserRole { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}