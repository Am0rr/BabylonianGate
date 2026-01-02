using BG.Domain.Enums;

namespace BG.App.DTOs;

public record WeaponResponse(
    Guid Id,
    string Status,
    double? Condition
);