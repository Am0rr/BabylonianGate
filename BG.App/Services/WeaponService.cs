using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class WeaponService : IWeaponService
{
    private readonly IUnitOfWork _unitOfWork;

    public WeaponService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(Guid? Id, string Error)> CreateAsync(CreateWeaponRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateAsync(UpdateWeaponRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<string> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<WeaponResponse?> GetWeaponByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}