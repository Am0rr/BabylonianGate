using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Enums;
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
        if (!Enum.TryParse<WeaponType>(request.Type, true, out var weaponType))
        {
            return (null, $"Invalid weapon type: '{request.Type}'. Allowed values: {string.Join(", ", Enum.GetNames(typeof(WeaponType)))}");
        }

        var (weapon, error) = Weapon.Create(
            request.CodeName,
            request.SerialNumber,
            request.Caliber,
            weaponType
        );

        if (weapon is null)
        {
            return (null, error);
        }

        await _unitOfWork.Weapons.AddAsync(weapon);

        var (log, _) = OperationLog.Create("Create", $"Claimed weapon {weapon.Codename}, with SN {weapon.SerialNumber}", weapon.Id);

        if (log != null)
        {
            await _unitOfWork.Logs.AddAsync(log);
        }

        await _unitOfWork.CompleteAsync();

        return (weapon.Id, string.Empty);
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
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(id);

        if (weapon is null)
        {
            return null;
        }

        return new WeaponResponse(
            weapon.Id,
            weapon.Status.ToString(),
            weapon.Condition
        );
    }

    public async Task<List<WeaponResponse>> GetAllAsync()
    {
        var weapons = await _unitOfWork.Weapons.GetAllAsync();

        return weapons.Select(w => new WeaponResponse(
            w.Id,
            w.Status.ToString(),
            w.Condition
        )).ToList();
    }
}