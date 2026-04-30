using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.Clients;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Entities;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext context;

    public ClientService(AppDbContext _context)
    {
        context = _context;
    }

    public async Task<List<ClientResponseDto>> GetAllAsync()
    {
        return await context.Clients.AsNoTracking().Select(c => new ClientResponseDto
        {
            Id = c.Id,
            Identification = c.Identification,
            FullName = c.FullName,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            Address = c.Address,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt
        }).ToListAsync();
    }

    public async Task<ClientResponseDto?> GetById(int id)
    {
        return await context.Clients
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ClientResponseDto
            {
                Id = c.Id,
                Identification = c.Identification,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClientResponseDto> Create(CreateClientDto dto)
    {
        var exist = await context.Clients.AnyAsync(c => c.Identification == dto.Identification);

        if(exist)
            throw new Exception("Ya existe un cliente registrado con esa identificación");
        
        var client = new Client
        {
            Identification = dto.Identification,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = true
        };

        context.Clients.Add(client);
        await context.SaveChangesAsync();

        return new ClientResponseDto
        {
            Id = client.Id,
            Identification = client.Identification,
            FullName = client.FullName,
            PhoneNumber = client.PhoneNumber,
            Email = client.Email,
            Address = client.Address,
            IsActive = client.IsActive,
            CreatedAt = client.CreatedAt
        };
    }

    public async Task<bool> Update(int id, UpdateClientDto dto)
    {
        var client = await context.Clients.FindAsync(id);

        if (client == null)
            return false;

        var duplicatedIdentification = await context.Clients
            .AnyAsync(c => c.Identification == dto.Identification && c.Id != id);
        
        if (duplicatedIdentification)
            throw new Exception("Ya existe otro cliente con esa identificación");
        
        client.Identification = dto.Identification;
        client.FullName = dto.FullName;
        client.PhoneNumber = dto.PhoneNumber;
        client.Email = dto.Email;
        client.Address = dto.Address;
        client.IsActive = dto.IsActive;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var client = await context.Clients.Include(c => c.Pets).FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
            return false;
        
        if (client.Pets.Any())
            throw new Exception("No se puede eliminar el cliente porque tiene mascotas registradas");

        context.Clients.Remove(client);
        await context.SaveChangesAsync();

        return true;
    }
}