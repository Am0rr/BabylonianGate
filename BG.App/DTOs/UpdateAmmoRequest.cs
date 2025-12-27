using BG.Domain.Enums;

namespace BG.App.DTOs;

public record UpdateAmmoRequest(
    Guid Id,
    int Quantity
);