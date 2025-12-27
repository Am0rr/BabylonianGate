

using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface IAmmoService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateAmmoRequest request);
    Task<string> DeleteAsync(Guid id);
    Task<string> UpdateAsync(UpdateAmmoRequest request);
}