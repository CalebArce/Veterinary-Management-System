using VeterinarySystem.Application.DTOs.Pets;

namespace VeterinarySystem.Application.Interfaces;

public interface IPetService
{
    Task<List<PetResponseDto>> GetAllAsync();
    Task<List<PetResponseDto>> GetByClientId(int clientId);
    Task<PetResponseDto?> GetById(int id);
    Task<PetResponseDto> Create(CreatePetDto dto);
    Task<bool> Update(int id, UpdatePetDto dto);
    Task<bool> Delete(int id);
}