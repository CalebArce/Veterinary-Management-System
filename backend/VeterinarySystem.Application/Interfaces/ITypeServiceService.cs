using VeterinarySystem.Application.DTOs.TypeServices;

namespace VeterinarySystem.Application.Interfaces;

public interface ITypeServiceService
{
    Task<List<TypeServiceResponseDto>> GetAllAsync();
    Task<TypeServiceResponseDto?> GetById(int id);
    Task<TypeServiceResponseDto> Create(CreateTypeServiceDto dto);
    Task<bool> Update(int id, UpdateTypeServiceDto dto);
    Task<bool> Delete(int id);
}