using VeterinarySystem.Application.DTOs.Clients;

namespace VeterinarySystem.Application.Interfaces;

public interface IClientService
{
    Task<List<ClientResponseDto>> GetAllAsync();
    Task<ClientResponseDto?> GetById(int id);
    Task<ClientResponseDto> Create(CreateClientDto dto);
    Task<bool> Update(int id, UpdateClientDto dto);
    Task<bool> Delete(int id);
}