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
        RuleFor(w => w.SerialNumber).NotEmpty().MaximumLength(50)
                .Matches("^[a-zA-Z0-9-]+$").WithMessage("Serial number can only contain letters, numbers and dashes");
        RuleFor(w => w.Caliber).NotEmpty().MaximumLength(20);
    }
}