using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface IWeaponService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateWeaponRequest request);
    Task<string> DeleteAsync(Guid weaponId);
    Task<string> UpdateDetailsAsync(UpdateWeaponDetailsRequest request);
    Task<string> IssueWeaponAsync(Guid weaponId, Guid soldierId);
    Task<string> ReturnToStorageAsync(Guid weaponId, int roundsFired);
    Task<string> SendToMaintenanceAsync(Guid weaponId);
    Task<string> ReportMissingAsync(Guid weaponId);
    Task<WeaponResponse?> GetWeaponByIdAsync(Guid weaponId);
    Task<List<WeaponResponse>> GetAllAsync();
}