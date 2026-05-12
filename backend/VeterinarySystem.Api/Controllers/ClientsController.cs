using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinarySystem.Api.Helpers;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.DTOs.Clients;
using VeterinarySystem.Application.Interfaces;
using VeterinarySystem.Application.Security;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
public class ClientsController : ControllerBase
{
    private readonly IClientService clientService;
    private readonly IValidator<CreateClientDto> createValidator;
    private readonly IValidator<UpdateClientDto> updateValidator;

    public ClientsController(IClientService _clientService, IValidator<CreateClientDto> _createValidator, IValidator<UpdateClientDto> _updateValidator)
    {
        clientService = _clientService;
        createValidator = _createValidator;
        updateValidator = _updateValidator;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
    public async Task<IActionResult> GetAll()
    {
        var clients = await clientService.GetAllAsync();

        if (clients == null)
            return NotFound(ApiResponse<object>.Fail("No hay clientes registrados"));
        
        return Ok(ApiResponse<object>.Ok(clients));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = AppRoles.AdminVeterinarianOrAssistant)]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await clientService.GetById(id);

        if (client == null)
            return NotFound(ApiResponse<object>.Fail("Cliente no encontrado"));

        return Ok(ApiResponse<object>.Ok(client));
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Assistant)]
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        var client = await clientService.Create(dto);

        var validation = await ValidationHelper.ValidateAsync(createValidator, dto);
        if (validation != null) return validation;
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = client.Id }, 
            ApiResponse<object>.Ok(client, "Cliente registrado"));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto dto)
    {
        var updated = await clientService.Update(id, dto);

        var validation = await ValidationHelper.ValidateAsync(updateValidator, dto);
        if (validation != null) return validation;

        if (!updated)
            return NotFound(ApiResponse<object>.Fail("Cliente no encontrado"));

        return Ok(ApiResponse<object>.Ok(null!, "Datos actualizados"));

    }

    /// <summary>
    /// Security:
    /// Only administrators can perform delete operations
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await clientService.Delete(id);

        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Cliente no encontrado"));

        return Ok(ApiResponse<object>.Ok(null!, "Cliente eliminado"));

    }
}