using BG.App.DTOs;
using BG.Domain.Entities;

namespace BG.App.Interfaces;

public interface IAmmoService
{
    Task<Guid> CreateAsync(CreateAmmoRequest request);
    Task DeleteAsync(Guid crateId);
    Task UpdateDetailsAsync(UpdateAmmoDetailsRequest request);
    Task IssueAmmoAsync(IssueAmmoRequest request);
    Task RestockAsync(RestockAmmoRequest request);
    Task AuditInventoryAsync(Guid crateId, int actualQuantity);
    Task<AmmoResponse?> GetCrateByIdAsync(Guid crateId);
    Task<List<AmmoResponse>> GetAllAsync();
}