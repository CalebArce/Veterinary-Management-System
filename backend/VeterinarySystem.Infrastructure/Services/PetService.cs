using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.Pets;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class PetService : IPetService
{
    private readonly AppDbContext context;

    public PetService(AppDbContext _context)
    {
        context = _context;
    }

    public async Task<List<PetResponseDto>> GetAllAsync()
    {
        return await context.Pets
            .AsNoTracking()
            .Include(p => p.client)
            .Select(p => new PetResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Species = p.Species,
                Breed = p.Breed,
                Age = p.Age,
                Weight = p.Weight,
                IsActive = p.IsActive,
                ClientId = p.ClientId,
                ClientName = p.client.FullName,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<PetResponseDto>> GetByClientId(int clientId)
    {
        return await context.Pets
            .AsNoTracking()
            .Where(p => p.ClientId == clientId)
            .Select(p => new PetResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Species = p.Species,
                Breed = p.Breed,
                Age = p.Age,
                Weight = p.Weight,
                IsActive = p.IsActive,
                ClientId = p.ClientId,
                ClientName = p.client.FullName,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PetResponseDto?> GetById(int id)
    {
        return await context.Pets
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PetResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Species = p.Species,
                Breed = p.Breed,
                Age = p.Age,
                Weight = p.Weight,
                IsActive = p.IsActive,
                ClientId = p.ClientId,
                ClientName = p.client.FullName,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PetResponseDto> Create(CreatePetDto dto)
    {
        var client = await context.Clients
            .FirstOrDefaultAsync(c => c.Id == dto.ClientId && c.IsActive);

        if (client == null)
            throw new Exception("El cliente no existe o está inactivo");

        if (dto.Age < 0)
            throw new Exception("La edad de la mascota no puede ser negativa");

        if (dto.Weight <= 0)
            throw new Exception("El peso de la mascota debe ser mayor a cero");

        var pet = new Pet
        {
            ClientId = dto.ClientId,
            Name = dto.Name,
            Species = dto.Species,
            Breed = dto.Breed,
            Age = dto.Age,
            Weight = dto.Weight,
            IsActive = true
        };

        context.Pets.Add(pet);
        await context.SaveChangesAsync();

        return new PetResponseDto
        {
            Id = pet.Id,
            Name = pet.Name,
            Species = pet.Species,
            Breed = pet.Breed,
            Age = pet.Age,
            Weight = pet.Weight,
            IsActive = pet.IsActive,
            ClientId = pet.ClientId,
            ClientName = client.FullName,
            CreatedAt = pet.CreatedAt
        };
    }

    public async Task<bool> Update(int id, UpdatePetDto dto)
    {
        var pet = await context.Pets.FindAsync(id);

        if (pet == null)
            return false;

        if (dto.Age < 0)
            throw new Exception("La edad de la mascota no puede ser negativa");

        if (dto.Weight <= 0)
            throw new Exception("El peso de la mascota debe ser mayor a cero");

        pet.Name = dto.Name;
        pet.Species = dto.Species;
        pet.Breed = dto.Breed;
        pet.Age = dto.Age;
        pet.Weight = dto.Weight;
        pet.IsActive = dto.IsActive;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var pet = await context.Pets
            .Include(p => p.petServices)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pet == null)
            return false;

        if (pet.petServices.Any())
            throw new Exception("No se puede eliminar la mascota porque tiene servicios asociados");

        context.Pets.Remove(pet);
        await context.SaveChangesAsync();

        return true;
    }
}