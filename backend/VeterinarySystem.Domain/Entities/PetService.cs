using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Domain.Entities;

public class PetService : AuditableEntity
{
    public int Id { get; set; }

    public int PetId { get; set; }
    public Pet pet { get; set; } = null!;

    public int TypeServiceId { get; set; }
    public TypeService typeService { get; set; } = null!;

    public DateTime ServiceDate { get; set; } = DateTime.UtcNow;

    public decimal FinalPrice { get; set; }

    public int DurationMinutes { get; set; }

    public BillingStatus billingStatus { get; set; } = BillingStatus.Pending;

    public string? Notes { get; set; }

}