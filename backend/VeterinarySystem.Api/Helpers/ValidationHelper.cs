using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VeterinarySystem.Application.Common;

namespace VeterinarySystem.Api.Helpers;

public static class ValidationHelper
{
    public static async Task<IActionResult?> ValidateAsync<T>(
        IValidator<T> validator,
        T dto)
    {
        var result = await validator.ValidateAsync(dto);

        if (result.IsValid)
            return null;

        var errors = result.Errors
            .Select(e => e.ErrorMessage)
            .ToList();

        return new BadRequestObjectResult(
            ApiResponse<object>.Fail(string.Join(" | ", errors))
        );
    }
}