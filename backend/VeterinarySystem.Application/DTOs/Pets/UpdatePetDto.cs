namespace VeterinarySystem.Application.DTOs.Pets;

public class UpdatePetDto
{
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;

    public int Age { get; set; }
    public decimal Weight { get; set; }

    public bool IsActive { get; set; }
}