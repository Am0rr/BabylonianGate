

namespace BG.App.DTOs;

public record AmmoResponse(
    Guid Id,
    string LotNumber,
    string Caliber,
    int Quantity,
    string Type
);