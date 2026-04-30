using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Application.Security;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Veterinarian + "," + AppRoles.Assistant)]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService _dashboardService)
    {
        dashboardService = _dashboardService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var summary = await dashboardService.GetSummaryAsync(startDate, endDate);
        return Ok(ApiResponse<object>.Ok(summary));
    }

    [HttpGet("monthly-revenue")]
    public async Task<IActionResult> GetMonthlyRevenue(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var data = await dashboardService.GetMonthlyRevenueAsync(startDate, endDate);

        return Ok(ApiResponse<object>.Ok(data));
    }
}