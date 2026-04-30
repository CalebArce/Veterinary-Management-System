using VeterinarySystem.Application.DTOs.Dashboard;

namespace VeterinarySystem.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync(DateTime? startDate, DateTime? endDate);
    Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(DateTime? startDate, DateTime? endDate);
}