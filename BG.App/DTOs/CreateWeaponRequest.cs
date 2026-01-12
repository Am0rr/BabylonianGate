using BG.Domain.Enums;

namespace BG.App.DTOs;

public record CreateWeaponRequest(
    string CodeName,
    string SerialNumber,
    string Caliber,
    string Type
);