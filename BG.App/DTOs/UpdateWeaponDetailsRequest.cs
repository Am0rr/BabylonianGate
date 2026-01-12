

namespace BG.App.DTOs;

public record UpdateWeaponDetailsRequest(
    Guid Id,
    string Codename,
    string SerialNumber,
    string Caliber
);