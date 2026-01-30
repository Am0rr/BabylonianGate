

namespace BG.App.DTOs;

public record IssueWeaponRequest(
    Guid WeaponId,
    Guid SoldierId
);