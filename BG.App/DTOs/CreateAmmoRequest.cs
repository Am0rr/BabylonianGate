using BG.Domain.Enums;

namespace BG.App.DTOs;

public record CreateAmmoRequest(
    string LotNumber,
    string Caliber,
    int Quantity,
    string Type
);