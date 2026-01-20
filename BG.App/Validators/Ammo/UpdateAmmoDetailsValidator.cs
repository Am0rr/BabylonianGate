using BG.App.DTOs;
using BG.Domain.Enums;
using FluentValidation;

namespace BG.App.Validators.Ammo;

public class UpdateAmmoDetailsValidator : AbstractValidator<UpdateAmmoDetailsRequest>
{
    public UpdateAmmoDetailsValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.LotNumber).NotEmpty().MaximumLength(50);
        RuleFor(a => a.Caliber).NotEmpty().MaximumLength(20);
        RuleFor(a => a.Type).IsEnumName(typeof(AmmoType), caseSensitive: false);
    }
}