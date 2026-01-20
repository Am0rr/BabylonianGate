using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface IWeaponService
{
    Task<Guid> CreateAsync(CreateWeaponRequest request);
    Task DeleteAsync(Guid weaponId);
    Task UpdateDetailsAsync(UpdateWeaponDetailsRequest request);
    Task IssueWeaponAsync(Guid weaponId, Guid soldierId);
    Task ReturnToStorageAsync(Guid weaponId, int roundsFired);
    Task SendToMaintenanceAsync(Guid weaponId);
    Task ReportMissingAsync(Guid weaponId);
    Task<WeaponResponse?> GetWeaponByIdAsync(Guid weaponId);
    Task<List<WeaponResponse>> GetAllAsync();
}