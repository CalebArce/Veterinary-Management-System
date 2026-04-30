using VeterinarySystem.Application.DTOs.PetServices;

namespace VeterinarySystem.Application.Interfaces;

public interface IPetServiceManager
{
    Task<List<PetServiceResponseDto>> GetAllAsync();
    Task<List<PetServiceResponseDto>> GetByPetId(int PetId);
    Task<PetServiceResponseDto?> GetById(int id);
    Task<PetServiceResponseDto> Create(CreatePetServiceDto dto);
    Task<bool> Update(int id, UpdateBillingStatusDto dto);
}