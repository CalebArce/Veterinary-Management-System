using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VeterinarySystem.Api.Helpers;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.DTOs.TypeServices;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Application.Security;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TypeServicesController : ControllerBase
{
    private readonly ITypeServiceService typeServiceService;
    private readonly IValidator<CreateTypeServiceDto> createValidator;
    private readonly IValidator<UpdateTypeServiceDto> updateValidator;

    public TypeServicesController(ITypeServiceService _typeServiceService, IValidator<CreateTypeServiceDto> _createValidator, IValidator<UpdateTypeServiceDto> _updateValidator)
    {
        typeServiceService = _typeServiceService;
        createValidator = _createValidator;
        updateValidator = _updateValidator;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
    public async Task<IActionResult> GetAll()
    {
        var services = await typeServiceService.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(services));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
    public async Task<IActionResult> GetById(int id)
    {
        var service = await typeServiceService.GetById(id);

        if (service == null)
            return NotFound(ApiResponse<object>.Fail("Tipo de servicio no encontrado"));

        return Ok(ApiResponse<object>.Ok(service));
    }

    [HttpPost]
    [EnableRateLimiting("SensitivePolicy")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateTypeServiceDto dto)
    {
        var service = await typeServiceService.Create(dto);

        var validation = await ValidationHelper.ValidateAsync(createValidator, dto);
        if (validation != null) return validation;

        return CreatedAtAction(
                nameof(GetById), 
                new { id = service.Id }, 
                ApiResponse<object>.Ok(service, "Tipo de servicio creado")
        );
    }

    [HttpPut("{id:int}")]
    [EnableRateLimiting("SensitivePolicy")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTypeServiceDto dto)
    {
        var updated = await typeServiceService.Update(id, dto);

        var validation = await ValidationHelper.ValidateAsync(updateValidator, dto);
        if (validation != null) return validation;

        if (!updated)
            return NotFound(ApiResponse<object>.Fail("Tipo de servicio no encontrado"));

        return Ok(ApiResponse<object>.Ok(null!, "Tipo de servicio actualizado"));
    }

    /// <summary>
    /// Security:
    /// Only administrators can perform delete operations
    /// </summary>
    [HttpDelete("{id:int}")]
    [EnableRateLimiting("SensitivePolicy")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await typeServiceService.Delete(id);

        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Tipo de servicio no encontrado"));

        return Ok(ApiResponse<object>.Ok(null!, "Tipo de servicio eliminado"));
    }
}