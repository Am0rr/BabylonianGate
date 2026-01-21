using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface IWeaponService
{
    Task<Guid> CreateAsync(CreateWeaponRequest request);
    Task DeleteAsync(Guid weaponId);
    Task UpdateDetailsAsync(UpdateWeaponDetailsRequest request);
    Task IssueWeaponAsync(IssueWeaponRequest request);
    Task ReturnToStorageAsync(ReturnWeaponToStorageRequest request);
    Task SendToMaintenanceAsync(SendWeaponToMaintenanceRequest request);
    Task ReportMissingAsync(ReportWeaponMissingRequest request);
    Task<WeaponResponse?> GetWeaponByIdAsync(Guid weaponId);
    Task<List<WeaponResponse>> GetAllAsync();
}