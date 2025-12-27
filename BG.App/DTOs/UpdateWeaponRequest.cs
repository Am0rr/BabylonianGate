

using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateWeaponRequest(
    Guid Id,
    WeaponStatus Status,
    double? Condition
);