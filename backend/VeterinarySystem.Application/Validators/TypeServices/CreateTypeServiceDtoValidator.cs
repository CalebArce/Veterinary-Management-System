using FluentValidation;
using VeterinarySystem.Application.DTOs.TypeServices;

namespace VeterinarySystem.Application.Validators.TypeServices;

public class CreateTypeServiceDtoValidator : AbstractValidator<CreateTypeServiceDto>
{
    public CreateTypeServiceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del servicio es obligatorio")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(250);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("La duración debe ser mayor a cero");
    }
}