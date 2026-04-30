using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.TypeServices;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class TypeServiceService : ITypeServiceService
{
    private readonly AppDbContext context;

    public TypeServiceService(AppDbContext _context)
    {
        context = _context;
    }

    public async Task<List<TypeServiceResponseDto>> GetAllAsync()
    {
        return await context.TypeServices
            .AsNoTracking()
            .Select(ts => new TypeServiceResponseDto
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
                Price = ts.Price,
                DurationMinutes = ts.DurationMinutes,
                IsActive = ts.IsActive,
                CreatedAt = ts.CreatedAt
            })
            .ToListAsync();
    }

     public async Task<TypeServiceResponseDto?> GetById(int id)
    {
        return await context.TypeServices
            .AsNoTracking()
            .Where(ts => ts.Id == id)
            .Select(ts => new TypeServiceResponseDto
            {
                Id = ts.Id,
                Name = ts.Name,
                Description = ts.Description,
                Price = ts.Price,
                DurationMinutes = ts.DurationMinutes,
                IsActive = ts.IsActive,
                CreatedAt = ts.CreatedAt
            })
            .FirstOrDefaultAsync();       
    }

    public async Task<TypeServiceResponseDto> Create(CreateTypeServiceDto dto)
    {
        if (dto.Price <= 0)
            throw new Exception("El precio del servicio debe ser mayor a cero");

        if (dto.DurationMinutes <= 0)
            throw new Exception("La duración del servicio debe ser mayor a cero");

        var exists = await context.TypeServices
            .AnyAsync(ts => ts.Name == dto.Name);

        if (exists)
            throw new Exception("Ya existe un tipo de servicio con ese nombre");

        var typeService = new TypeService
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            DurationMinutes = dto.DurationMinutes,
            IsActive = true
        };

        context.TypeServices.Add(typeService);
        await context.SaveChangesAsync();

        return new TypeServiceResponseDto
        {
            Id = typeService.Id,
            Name = typeService.Name,
            Description = typeService.Description,
            Price = typeService.Price,
            DurationMinutes = typeService.DurationMinutes,
            IsActive = typeService.IsActive,
            CreatedAt = typeService.CreatedAt
        };
    }

    public async Task<bool> Update(int id, UpdateTypeServiceDto dto)
    {
        var typeService = await context.TypeServices.FindAsync(id);

        if (typeService == null)
            return false;

        if (dto.Price <= 0)
            throw new Exception("El precio del servicio debe ser mayor a cero");

        if (dto.DurationMinutes <= 0)
            throw new Exception("La duración del servicio debe ser mayor a cero");

        var exists = await context.TypeServices
            .AnyAsync(ts => ts.Name == dto.Name && ts.Id != id);

        if (exists)
            throw new Exception("Ya existe otro tipo de servicio con ese nombre");

        typeService.Name = dto.Name;
        typeService.Description = dto.Description;
        typeService.Price = dto.Price;
        typeService.DurationMinutes = dto.DurationMinutes;
        typeService.IsActive = dto.IsActive;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var typeService = await context.TypeServices
            .Include(ts => ts.petServices)
            .FirstOrDefaultAsync(ts => ts.Id == id);

        if (typeService == null)
            return false;

        if (typeService.petServices.Any())
            throw new Exception("No se puede eliminar el tipo de servicio porque ya fue utilizado");

        context.TypeServices.Remove(typeService);
        await context.SaveChangesAsync();

        return true;
    }
}