using BG.App.DTOs;
using BG.Domain.Entities;

namespace BG.App.Interfaces;

public interface IWeaponService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateWeaponRequest request);
    Task<string> DeleteAsync(Guid id);
    Task<string> UpdateAsync(UpdateWeaponRequest request);
    Task<WeaponResponse?> GetWeaponByIdAsync(Guid id);
    Task<List<WeaponResponse>> GetAllAsync();
}