using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateAmmoDetailsRequest(
    Guid Id,
    string Caliber,
    string Type
);