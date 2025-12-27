using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface ISoldierService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateSoldierRequest request);
    Task<string> UpdateAsync(UpdateSoldierRequest request);
}