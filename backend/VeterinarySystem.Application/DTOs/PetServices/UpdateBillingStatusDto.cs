using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Application.DTOs.PetServices;

public class UpdateBillingStatusDto
{
    public BillingStatus billingStatus { get; set; }
}