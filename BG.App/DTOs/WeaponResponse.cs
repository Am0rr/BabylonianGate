using BG.Domain.Enums;

namespace BG.App.DTOs;

public record WeaponResponse(
    Guid Id,
    string CodeName,
    string SerialNumber,
    string Caliber,
    string Type,
    string Status,
    double? Condition,
    Guid? IssuedToSoldierId
);