

namespace BG.App.DTOs;

public record AuditAmmoInventoryRequest(
    Guid CrateId,
    int ActualQuantity
);