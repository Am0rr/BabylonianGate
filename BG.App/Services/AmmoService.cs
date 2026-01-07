using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class AmmoService : IAmmoService
{
    private readonly IUnitOfWork _unitOfWork;

    public AmmoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(Guid? Id, string Error)> CreateAsync(CreateAmmoRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteAsync(Guid crateId)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateDetailsAsync(UpdateAmmoDetailsRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<string> IssueAmmoAsync(Guid crateId, int amount)
    {
        throw new NotImplementedException();
    }

    public async Task<string> RestockAsync(Guid crateId, int amount)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AuditInventoryAsync(Guid crateId, int actualQuantity)
    {
        throw new NotImplementedException();
    }

    public async Task<AmmoResponse?> GetCrateByIdAsync(Guid crateId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AmmoResponse>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}