

using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateWeaponRequest(
    Guid Id,
    double? Condition,
    string Status
);