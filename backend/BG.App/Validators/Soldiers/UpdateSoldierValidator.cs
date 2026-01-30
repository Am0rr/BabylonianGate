using BG.App.DTOs;
using BG.Domain.Enums;
using FluentValidation;

namespace BG.App.Validators.Soldiers;

public class UpdateSoldierValidator : AbstractValidator<UpdateSoldierRequest>
{
    public UpdateSoldierValidator()
    {
        RuleFor(s => s.Id).NotEmpty();
        RuleFor(s => s.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(s => s.LastName).NotEmpty().MaximumLength(50);
        RuleFor(s => s.Rank).IsEnumName(typeof(SoldierRank), caseSensitive: false)
                .WithMessage($"Invalid Rank. Allowed: {string.Join(", ", Enum.GetNames(typeof(SoldierRank)))}");
    }
}