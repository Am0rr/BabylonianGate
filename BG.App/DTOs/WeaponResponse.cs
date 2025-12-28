using BG.Domain.Enums;

namespace BG.App.DTOs;

public record WeaponResponse(
    Guid Id,
    WeaponStatus Status,
    double? Condition
);