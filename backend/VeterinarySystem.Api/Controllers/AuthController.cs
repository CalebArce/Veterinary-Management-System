using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VeterinarySystem.Api.Helpers;
using VeterinarySystem.Application.Common;
using VeterinarySystem.Application.DTOs.Auth;
using VeterinarySystem.Application.Interfaces;

namespace VeterinarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly IValidator<RegisterUserDto> registerValidator;
    private readonly IValidator<LoginRequestDto> loginValidator;

    public AuthController(IAuthService _authService, IValidator<RegisterUserDto> _registerValidator, IValidator<LoginRequestDto> _loginValidator)
    {
        authService = _authService;
        registerValidator = _registerValidator;
        loginValidator = _loginValidator;
    }

    [HttpPost("register")]
    [EnableRateLimiting("AuthPolicy")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var validation = await ValidationHelper.ValidateAsync(registerValidator, dto);
        if (validation != null) return validation;

        var response = await authService.RegisterAsync(dto);

        return Ok(ApiResponse<object>.Ok(response, "Usuario registrado"));
    }

    [HttpPost("login")]
    [EnableRateLimiting("AuthPolicy")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var validation = await ValidationHelper.ValidateAsync(loginValidator, dto);
        if (validation != null) return validation;

        var response = await authService.LoginAsync(dto);

        return Ok(ApiResponse<object>.Ok(response, "Inicio de Sesión correcto, Bienvenido!"));
    }

    [HttpPost("refresh")]
    [EnableRateLimiting("AuthPolicy")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var response = await authService.RefreshAsync(dto);

        return Ok(ApiResponse<object>.Ok(response, "Token renovado correctamente"));
    }

    [HttpPost("logout")]
    [EnableRateLimiting("AuthPolicy")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto dto)
    {
        await authService.LogoutAsync(dto);

        return Ok(ApiResponse<object>.Ok(null!, "Sesión cerrada correctamente"));
    }
}