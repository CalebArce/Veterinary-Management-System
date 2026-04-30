namespace VeterinarySystem.Domain.Entities;
public class Client : AuditableEntity
{
    public int Id { get; set; }

    public string Identification { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;


    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}