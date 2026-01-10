using BG.App.DTOs;
using BG.Domain.Entities;

namespace BG.App.Interfaces;

public interface IAmmoService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateAmmoRequest request);
    Task<string> DeleteAsync(Guid crateId);
    Task<string> UpdateDetailsAsync(UpdateAmmoDetailsRequest request);
    Task<string> IssueAmmoAsync(IssueAmmoRequest request);
    Task<string> RestockAsync(RestockAmmoRequest request);
    Task<string> AuditInventoryAsync(Guid crateId, int actualQuantity);
    Task<AmmoResponse?> GetCrateByIdAsync(Guid crateId);
    Task<List<AmmoResponse>> GetAllAsync();
}