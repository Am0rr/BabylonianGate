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

    public async Task<string> UpdateAsync(UpdateAmmoRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<AmmoResponse?> GetCrateByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}