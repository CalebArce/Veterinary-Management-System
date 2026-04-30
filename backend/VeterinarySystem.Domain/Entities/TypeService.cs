namespace VeterinarySystem.Domain.Entities;

public class TypeService : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int DurationMinutes { get; set; }

    public bool IsActive { get; set; } = true;


    public ICollection<PetService> petServices { get; set; } = new List<PetService>();
}