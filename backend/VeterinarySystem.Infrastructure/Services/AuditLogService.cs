using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.AuditLogs;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;
using VeterinarySystem.Domain.Enums;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext context;
    private readonly ICurrentUserService currentUserService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuditLogService(
        AppDbContext _context,
        ICurrentUserService _currentUserService,
        IHttpContextAccessor _httpContextAccessor)
    {
        context = _context;
        currentUserService = _currentUserService;
        httpContextAccessor = _httpContextAccessor;
    }

    public async Task RegisterAsync(
        AuditAction action,
        string entityName,
        int? entityId,
        string description)
    {
        var auditLog = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Description = description,
            UserEmail = currentUserService.Email ?? "Anonymous",
            UserRole = currentUserService.Role ?? "Unknown",
            IpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        context.AuditLogs.Add(auditLog);
        await context.SaveChangesAsync();
    }

    public async Task<List<AuditLogResponseDto>> GetAllAsync()
    {
        return await context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AuditLogResponseDto
            {
                Id = a.Id,
                Action = a.Action,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Description = a.Description,
                UserEmail = a.UserEmail,
                UserRole = a.UserRole,
                IpAddress = a.IpAddress,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<AuditLogResponseDto>> GetByEntityAsync(string entityName, int entityId)
    {
        return await context.AuditLogs
            .AsNoTracking()
            .Where(a => a.EntityName == entityName && a.EntityId == entityId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AuditLogResponseDto
            {
                Id = a.Id,
                Action = a.Action,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Description = a.Description,
                UserEmail = a.UserEmail,
                UserRole = a.UserRole,
                IpAddress = a.IpAddress,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
    }
}