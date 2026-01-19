using BG.App.DTOs;
using BG.Domain.Enums;
using FluentValidation;

namespace BG.App.Validators.Weapons;

public class UpdateWeaponDetailsValidator : AbstractValidator<UpdateWeaponDetailsRequest>
{
    public UpdateWeaponDetailsValidator()
    {
        RuleFor(w => w.Id).NotEmpty();
        RuleFor(w => w.Codename).NotEmpty().MaximumLength(100);
        RuleFor(w => w.SerialNumber).NotEmpty().MaximumLength(50);
        RuleFor(w => w.Caliber).NotEmpty().MaximumLength(20);
    }
}