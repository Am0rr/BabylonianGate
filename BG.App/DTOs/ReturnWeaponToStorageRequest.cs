

namespace BG.App.DTOs;

public record ReturnWeaponToStorageRequest(
    Guid WeaponId,
    int RoundsFired
);