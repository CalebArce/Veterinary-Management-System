using FluentValidation;
using VeterinarySystem.Application.DTOs.PetServices;

namespace VeterinarySystem.Application.Validators.PetServices;

public class CreatePetServiceDtoValidator : AbstractValidator<CreatePetServiceDto>
{
    public CreatePetServiceDtoValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0).WithMessage("Debe seleccionar una mascota válida");

        RuleFor(x => x.TypeServiceId)
            .GreaterThan(0).WithMessage("Debe seleccionar un tipo de servicio válido");

        RuleFor(x => x.ServiceDate)
            .NotEmpty().WithMessage("La fecha del servicio es obligatoria");

        RuleFor(x => x.Notes)
            .MaximumLength(500);
    }
}