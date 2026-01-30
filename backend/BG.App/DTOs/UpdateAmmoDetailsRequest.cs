using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateAmmoDetailsRequest(
    Guid Id,
    string LotNumber,
    string Caliber,
    string Type
);