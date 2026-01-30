using BG.App.DTOs;
using BG.Domain.Enums;
using FluentValidation;

namespace BG.App.Validators.Ammo;

public class CreateAmmoValidator : AbstractValidator<CreateAmmoRequest>
{
    public CreateAmmoValidator()
    {
        RuleFor(a => a.LotNumber).NotEmpty().MaximumLength(50);
        RuleFor(a => a.Caliber).NotEmpty().MaximumLength(20);
        RuleFor(a => a.Quantity).GreaterThan(0);
        RuleFor(a => a.Type).IsEnumName(typeof(AmmoType), caseSensitive: false)
                .WithMessage($"Invalid Type. Allowed: {string.Join(", ", Enum.GetNames(typeof(AmmoType)))}");
    }
}