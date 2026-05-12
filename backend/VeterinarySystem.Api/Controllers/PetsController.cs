using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinarySystem.Api.Helpers;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.DTOs.Pets;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Application.Security;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PetsController : ControllerBase
{
    private readonly IPetService petService;
    private readonly IValidator<CreatePetDto> createValidator;
    private readonly IValidator<UpdatePetDto> updateValidator;

    public PetsController(IPetService _petService, IValidator<CreatePetDto> _createValidator, IValidator<UpdatePetDto> _updateValidator)
    {
        petService = _petService;
        createValidator = _createValidator;
        updateValidator = _updateValidator;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Assistant)]
    public async Task<IActionResult> GetAll()
    {
        var pets = await petService.GetAllAsync();

        if (pets == null)
            return NotFound(ApiResponse<object>.Fail("No hay registro de mascotas"));

        return Ok(ApiResponse<object>.Ok(pets));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Assistant)]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await petService.GetById(id);

        if (pet == null)
            return NotFound(ApiResponse<object>.Fail("Mascota no encontrada"));

        return Ok(ApiResponse<object>.Ok(pet));
    }

    [HttpGet("by-client/{clientId:int}")]
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Assistant)]
    public async Task<IActionResult> GetByClientId(int clientId)
    {
        var pets = await petService.GetByClientId(clientId);
        return Ok(ApiResponse<object>.Ok(pets));
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Veterinarian)]
    public async Task<IActionResult> Create([FromBody] CreatePetDto dto)
    {
        var pet = await petService.Create(dto);

        var validation = await ValidationHelper.ValidateAsync(createValidator, dto);
        if (validation != null) return validation;

        return CreatedAtAction(
                nameof(GetById), 
                new { id = pet.Id }, 
                ApiResponse<object>.Ok(pet, "Mascota registrada"));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePetDto dto)
    {
        var updated = await petService.Update(id, dto);

        var validation = await ValidationHelper.ValidateAsync(updateValidator, dto);
        if (validation != null) return validation;

        if (!updated)
            return NotFound(ApiResponse<object>.Fail("Mascota no encontrada"));

        return Ok(ApiResponse<object>.Ok(null!, "Mascota actualizada"));
    }

    /// <summary>
    /// Security:
    /// Only administrators can perform delete operations
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted = await petService.Delete(id);

        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Mascota no encontrada"));

        return Ok(ApiResponse<object>.Ok(null!, "Mascota eliminada"));
    }
}