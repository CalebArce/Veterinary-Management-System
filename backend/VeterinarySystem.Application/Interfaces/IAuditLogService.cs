using VeterinarySystem.Application.DTOs.AuditLogs;
using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Application.Interfaces;

public interface IAuditLogService
{
    Task RegisterAsync(
        AuditAction action,
        string entityName,
        int? entityId,
        string description);

    Task<List<AuditLogResponseDto>> GetAllAsync();

    Task<List<AuditLogResponseDto>> GetByEntityAsync(string entityName, int entityId);
}