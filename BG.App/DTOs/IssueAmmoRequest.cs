

namespace BG.App.DTOs;

public record IssueAmmoRequest(
    Guid CrateId,
    int Amount,
    Guid SoldierId
);