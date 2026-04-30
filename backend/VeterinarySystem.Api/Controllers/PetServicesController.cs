using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VeterinarySystem.Api.Helpers;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.DTOs.PetServices;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Application.Security;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PetServicesController : ControllerBase
{
    private readonly IPetServiceManager petServiceManager;
    private readonly IValidator<CreatePetServiceDto> createValidator;
    private readonly IValidator<UpdateBillingStatusDto> billingStatusValidator;

    public PetServicesController(IPetServiceManager _petServiceManager, IValidator<CreatePetServiceDto> _createValidator, IValidator<UpdateBillingStatusDto> _billingStatusValidator)
    {
        petServiceManager = _petServiceManager;
        createValidator = _createValidator;
        billingStatusValidator = _billingStatusValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var services = await petServiceManager.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(services));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
    public async Task<IActionResult> GetById(int id)
    {
        var service = await petServiceManager.GetById(id);

        if (service == null)
            return NotFound(ApiResponse<object>.Fail("Servicio de mascota no encontrado"));

        return Ok(ApiResponse<object>.Ok(service));
    }

    [HttpGet("by-pet/{petId:int}")]
    [Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
    public async Task<IActionResult> GetByPetId(int petId)
    {
        var services = await petServiceManager.GetByPetId(petId);
        return Ok(ApiResponse<object>.Ok(services));
    }

    [HttpPost]
    [EnableRateLimiting("SensitivePolicy")]
    [Authorize(Roles = AppRoles.AdminOrVeterinarian)]
    public async Task<IActionResult> Create([FromBody] CreatePetServiceDto dto)
    {
        var service = await petServiceManager.Create(dto);

        var validation = await ValidationHelper.ValidateAsync(createValidator, dto);
        if (validation != null) return validation;

        return CreatedAtAction(
                nameof(GetById), 
                new { id = service.Id }, 
                ApiResponse<object>.Ok(service, "Servicio asignado correctamente")
        );
    }

    [HttpPatch("{id:int}/billing-status")]
    [EnableRateLimiting("SensitivePolicy")]
    [Authorize(Roles = AppRoles.AdminOrVeterinarian)]
    public async Task<IActionResult> UpdateBillingStatus(int id, [FromBody] UpdateBillingStatusDto dto)
    {
        var updated = await petServiceManager.Update(id, dto);

        var validation = await ValidationHelper.ValidateAsync(billingStatusValidator, dto);
    if (validation != null) return validation;

        if (!updated)
            return NotFound(ApiResponse<object>.Fail("Servicio de mascota no encontrado"));

        return Ok(ApiResponse<object>.Ok(null!, "Estado de facturación actualizado correctamente."));
    }
}