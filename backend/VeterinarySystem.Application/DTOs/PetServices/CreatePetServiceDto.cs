namespace VeterinarySystem.Application.DTOs.PetServices;

public class CreatePetServiceDto
{
    public int PetId { get; set; }
    public int TypeServiceId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string? Notes { get; set; }
}