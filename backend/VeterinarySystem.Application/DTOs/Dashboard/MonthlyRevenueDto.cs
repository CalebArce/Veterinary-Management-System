namespace VeterinarySystem.Application.DTOs.Dashboard;

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal EstimatedRevenue { get; set; }
    public decimal PaidRevenue { get; set; }
}