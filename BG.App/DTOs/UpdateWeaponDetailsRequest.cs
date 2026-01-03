

namespace BG.App.DTOs;

public record UpdateWeaponDetailsRequest(
    Guid Id,
    string CodeName,
    string SerialNumber,
    string Caliber
);