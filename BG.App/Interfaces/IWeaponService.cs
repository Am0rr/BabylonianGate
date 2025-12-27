using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface IWeaponService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateWeaponRequest request);
    Task<string> DeleteAsync(Guid id);
    Task<string> UpdateAsync(UpdateWeaponRequest request);
}