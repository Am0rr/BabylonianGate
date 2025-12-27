using BG.Domain.Enums;

namespace BG.App.DTOs;

public record CreateWeaponRequest(
    string CodeName,
    string SerialNumber,
    WeaponType Type,
    string Caliber
);