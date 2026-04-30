using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Application.Security;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService auditLogService;

    public AuditLogsController(IAuditLogService _auditLogService)
    {
        auditLogService = _auditLogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var logs = await auditLogService.GetAllAsync();

        return Ok(ApiResponse<object>.Ok(logs));
    }

    [HttpGet("{entityName}/{entityId:int}")]
    public async Task<IActionResult> GetByEntity(string entityName, int entityId)
    {
        var logs = await auditLogService.GetByEntityAsync(entityName, entityId);

        return Ok(ApiResponse<object>.Ok(logs));
    }
}