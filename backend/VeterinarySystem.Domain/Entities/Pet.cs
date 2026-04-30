namespace VeterinarySystem.Domain.Entities;
public class Pet : AuditableEntity
{
    public int Id { get; set;}

    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;

    public int Age { get; set; }
    public decimal Weight { get; set; }

    public bool IsActive { get; set; } = true;


    public int ClientId { get; set; }
    public Client client { get; set; } = null!;

    public ICollection<PetService> petServices { get; set; } = new List<PetService>();
}
