using System.Data;
using FluentValidation;
using VeterinarySystem.Application.DTOs.Auth;

namespace VeterinarySystem.Application.Validators.Auth;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("El nombre completo es obligatorio")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres");
        
        RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio")
                .EmailAddress().WithMessage("El correo electrónico tiene un formato inválido")
                .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres");
        
        RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
                .Matches("[A-Z]").WithMessage("La contraseña debe tener al menos una mayúscula")
                .Matches("[a-z]").WithMessage("La contraseña debe tener al menos una minúscula")
                .Matches("[0-9]").WithMessage("La contraseña debe tener al menos un número")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un símbolo");
    }
}
