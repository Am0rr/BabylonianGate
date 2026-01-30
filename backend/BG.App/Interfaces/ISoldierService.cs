using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface ISoldierService
{
    Task<Guid> CreateAsync(CreateSoldierRequest request);
    Task UpdateAsync(UpdateSoldierRequest request);
    Task DeleteAsync(Guid soldierId);
    Task<SoldierResponse?> GetSoldierByIdAsync(Guid soldierId);
    Task<List<SoldierResponse>> GetAllAsync();
}