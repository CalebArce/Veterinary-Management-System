using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Application.DTOs.PetServices;

public class PetServiceResponseDto
{
    public int Id { get; set; }

    public int PetId { get; set; }
    public string PetName { get; set; } = string.Empty;

    public int TypeServiceId { get; set; }
    public string typeService { get; set; } = string.Empty;

    public DateTime ServiceDate { get; set; }
    public decimal FinalPrice { get; set; }
    public int DurationMinutes { get; set; }
    public BillingStatus billingStatus { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set;}
}