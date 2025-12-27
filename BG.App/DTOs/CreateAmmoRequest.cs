using BG.Domain.Enums;

namespace BG.App.DTOs;

public record CreateAmmoRequest(
    string Caliber,
    AmmoType Type,
    int Quantity
);