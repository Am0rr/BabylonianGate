using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateAmmoDetailsRequest(
    Guid CrateId,
    string LotNumber,
    string Caliber,
    string Type
);