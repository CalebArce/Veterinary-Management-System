using Microsoft.EntityFrameworkCore;
using VeterinarySystem.Application.DTOs.PetServices;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Domain.Enums;
using VeterinarySystem.Infrastructure.Data;

namespace VeterinarySystem.Infrastructure.Services;

public class PetServiceManager : IPetServiceManager
{
    private readonly AppDbContext context;
    private readonly IAuditLogService auditLogService;

    public PetServiceManager(AppDbContext _context, IAuditLogService _auditLogService)
    {
        context = _context;
        auditLogService = _auditLogService;
    }

    public async Task<List<PetServiceResponseDto>> GetAllAsync()
    {
        return await context.PetServices
            .AsNoTracking()
            .Include(ps => ps.pet)
            .Include(ps => ps.typeService)
            .Select(ps => new PetServiceResponseDto
            {
                Id = ps.Id,
                PetId = ps.PetId,
                PetName = ps.pet.Name,
                TypeServiceId = ps.TypeServiceId,
                typeService = ps.typeService.Name,
                ServiceDate = ps.ServiceDate,
                FinalPrice = ps.FinalPrice,
                DurationMinutes = ps.DurationMinutes,
                billingStatus = ps.billingStatus,
                Notes = ps.Notes,
                CreatedAt = ps.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<PetServiceResponseDto>> GetByPetId(int PetId)
    {
        return await context.PetServices
            .AsNoTracking()
            .Where(ps => ps.PetId == PetId)
            .Select(ps => new PetServiceResponseDto
            {
                Id = ps.Id,
                PetId = ps.PetId,
                PetName = ps.pet.Name,
                TypeServiceId = ps.TypeServiceId,
                typeService = ps.typeService.Name,
                ServiceDate = ps.ServiceDate,
                FinalPrice = ps.FinalPrice,
                DurationMinutes = ps.DurationMinutes,
                billingStatus = ps.billingStatus,
                Notes = ps.Notes,
                CreatedAt = ps.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PetServiceResponseDto?> GetById(int id)
    {
        return await context.PetServices
            .AsNoTracking()
            .Where(ps => ps.Id == id)
            .Select(ps => new PetServiceResponseDto
            {
                Id = ps.Id,
                PetId = ps.PetId,
                PetName = ps.pet.Name,
                TypeServiceId = ps.TypeServiceId,
                typeService = ps.typeService.Name,
                ServiceDate = ps.ServiceDate,
                FinalPrice = ps.FinalPrice,
                DurationMinutes = ps.DurationMinutes,
                billingStatus = ps.billingStatus,
                Notes = ps.Notes,
                CreatedAt = ps.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PetServiceResponseDto> Create(CreatePetServiceDto dto)
    {
        var pet = await context.Pets
            .FirstOrDefaultAsync(p => p.Id == dto.PetId && p.IsActive);

        if (pet == null)
            throw new Exception("La mascota no existe o está inactiva");

        var typeService = await context.TypeServices
            .FirstOrDefaultAsync(ts => ts.Id == dto.TypeServiceId && ts.IsActive);

        if (typeService == null)
            throw new Exception("El tipo de servicio no existe o está inactivo");

        var petService = new Domain.Entities.PetService
        {
            PetId = pet.Id,
            TypeServiceId = typeService.Id,
            ServiceDate = dto.ServiceDate,
            FinalPrice = typeService.Price,
            DurationMinutes = typeService.DurationMinutes,
            billingStatus = Domain.Enums.BillingStatus.Pending,
            Notes = dto.Notes
        };

        context.PetServices.Add(petService);
        await context.SaveChangesAsync();

        await auditLogService.RegisterAsync(
            AuditAction.CreatePetService,
            "PetService",
            petService.Id,
            $"Servicio asignado a la mascota {pet.Name}. Tipo de servicio: {typeService.Name}.");

        return new PetServiceResponseDto
        {
            Id = petService.Id,
            PetId = pet.Id,
            PetName = pet.Name,
            TypeServiceId = typeService.Id,
            typeService = typeService.Name,
            ServiceDate = petService.ServiceDate,
            FinalPrice = petService.FinalPrice,
            DurationMinutes = petService.DurationMinutes,
            billingStatus = petService.billingStatus,
            Notes = petService.Notes,
            CreatedAt = petService.CreatedAt
        };
    }

    public async Task<bool> Update(int id, UpdateBillingStatusDto dto)
    {
        var petService = await context.PetServices.FindAsync(id);

        if (petService == null)
            return false;

        petService.billingStatus = dto.billingStatus;

        await context.SaveChangesAsync();

        await auditLogService.RegisterAsync(
            AuditAction.UpdateBillingStatus,
            "PetService",
            petService.Id,
            $"Estado de facturación actualizado a {petService.billingStatus}.");

        return true;
    }
}