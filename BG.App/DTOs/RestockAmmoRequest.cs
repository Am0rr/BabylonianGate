

namespace BG.App.DTOs;

public record RestockAmmoRequest(
    Guid CrateId,
    int Amount,
    Guid SoldierId
);