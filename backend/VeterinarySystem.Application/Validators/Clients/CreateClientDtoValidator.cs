using FluentValidation;
using VeterinarySystem.Application.DTOs.Clients;

namespace VeterinarySystem.Application.Validators.Clients;

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.Identification)
            .NotEmpty().WithMessage("La identificación es obligatoria")
            .MaximumLength(30);

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es obligatorio")
            .MaximumLength(150);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El teléfono es obligatorio")
            .MaximumLength(30);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio")
            .EmailAddress().WithMessage("El correo no tiene un formato válido")
            .MaximumLength(150);

        RuleFor(x => x.Address)
            .MaximumLength(250);
    }
}