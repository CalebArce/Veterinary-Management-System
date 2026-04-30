namespace VeterinarySystem.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalClients { get; set; }
    public int TotalPets { get; set; }
    public int TotalTypeServices { get; set; }
    public int TotalPetServices { get; set; }

    public int PendingServices { get; set; }
    public int InvoicedServices { get; set; }
    public int PaidServices { get; set; }
    public int CancelledServices { get; set; }

    public decimal EstimatedRevenue { get; set; }
    public decimal PaidRevenue { get; set; }
}