using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.Dashboard;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Enums;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext context;

    public DashboardService(AppDbContext _context)
    {
        context = _context;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = context.PetServices.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.ServiceDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.ServiceDate <= endDate.Value);

        return new DashboardSummaryDto
        {
            TotalClients = await context.Clients.CountAsync(),
            TotalPets = await context.Pets.CountAsync(),
            TotalTypeServices = await context.TypeServices.CountAsync(),
            TotalPetServices = await query.CountAsync(),

            PendingServices = await query.CountAsync(x => x.billingStatus == BillingStatus.Pending),
            InvoicedServices = await query.CountAsync(x => x.billingStatus == BillingStatus.Invoiced),
            PaidServices = await query.CountAsync(x => x.billingStatus == BillingStatus.Paid),
            CancelledServices = await query.CountAsync(x => x.billingStatus == BillingStatus.Cancelled),

            EstimatedRevenue = await query
                .Where(x => x.billingStatus != BillingStatus.Cancelled)
                .SumAsync(x => x.FinalPrice),

            PaidRevenue = await query
                .Where(x => x.billingStatus == BillingStatus.Paid)
                .SumAsync(x => x.FinalPrice)
        };
    }

    public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = context.PetServices.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.ServiceDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.ServiceDate <= endDate.Value);

        var groupedData = await query
            .GroupBy(x => new
            {
                Year = x.ServiceDate.Year,
                Month = x.ServiceDate.Month
            })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                EstimatedRevenue = g
                    .Where(x => x.billingStatus != BillingStatus.Cancelled)
                    .Sum(x => x.FinalPrice),
                PaidRevenue = g
                    .Where(x => x.billingStatus == BillingStatus.Paid)
                    .Sum(x => x.FinalPrice)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return groupedData
            .Select(x => new MonthlyRevenueDto
            {
                Month = $"{x.Year}-{x.Month:00}",
                EstimatedRevenue = x.EstimatedRevenue,
                PaidRevenue = x.PaidRevenue
            })
            .ToList();
    }
}