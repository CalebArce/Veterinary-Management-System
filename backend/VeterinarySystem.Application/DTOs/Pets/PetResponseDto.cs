namespace VeterinarySystem.Application.DTOs.Pets;

public class PetResponseDto
{
    public int Id { get; set;}

    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;

    public int Age { get; set; }
    public decimal Weight { get; set; }

    public bool IsActive { get; set; }

    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

}