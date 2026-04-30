using FluentValidation;
using VeterinarySystem.Application.DTOs.PetServices;
using VeterinarySystem.Domain.Enums;

namespace VeterinarySystem.Application.Validators.PetServices;

public class UpdateBillingStatusDtoValidator : AbstractValidator<UpdateBillingStatusDto>
{
    public UpdateBillingStatusDtoValidator()
    {
        RuleFor(x => x.billingStatus)
            .Must(status => Enum.IsDefined(typeof(BillingStatus), status))
            .WithMessage("El estado de facturación no es válido.");
    }
}