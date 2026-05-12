using FluentValidation;
using VeterinarySystem.Application.DTOs.Pets;

namespace VeterinarySystem.Application.Validators.Pets;

/// <summary>
/// Ensures pet data integrity and validates required fields
/// </summary>
public class CreatePetDtoValidator : AbstractValidator<CreatePetDto>
{
    public CreatePetDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Debe seleccionar un cliente válido");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la mascota es obligatorio")
            .MaximumLength(100);

        RuleFor(x => x.Species)
            .NotEmpty().WithMessage("La especie es obligatoria")
            .MaximumLength(50);

        RuleFor(x => x.Breed)
            .MaximumLength(100);

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(0).WithMessage("La edad no puede ser negativa");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("El peso debe ser mayor a cero");
    }
}