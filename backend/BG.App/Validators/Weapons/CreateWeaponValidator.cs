using BG.App.DTOs;
using BG.Domain.Enums;
using FluentValidation;

namespace BG.App.Validators.Weapons;

public class CreateWeaponValidator : AbstractValidator<CreateWeaponRequest>
{
    public CreateWeaponValidator()
    {
        RuleFor(w => w.CodeName).NotEmpty().MaximumLength(100);
        RuleFor(w => w.SerialNumber).NotEmpty().MaximumLength(50)
                .Matches("^[a-zA-Z0-9-]+$").WithMessage("Serial number can only contain letters, numbers and dashes");
        RuleFor(w => w.Caliber).NotEmpty().MaximumLength(20);
        RuleFor(w => w.Type).IsEnumName(typeof(WeaponType), caseSensitive: false)
                .WithMessage($"Invalid Type. Allowed: {string.Join(", ", Enum.GetNames(typeof(WeaponType)))}");
    }
}