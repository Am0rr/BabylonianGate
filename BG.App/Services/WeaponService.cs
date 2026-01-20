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

    public async Task<Guid> CreateAsync(CreateWeaponRequest request)
    {
        var type = Enum.Parse<WeaponType>(request.Type, ignoreCase: true);

        var weapon = Weapon.Create(
            request.CodeName,
            request.SerialNumber,
            request.Caliber,
            type
        );

        await _unitOfWork.Weapons.AddAsync(weapon);

        var log = OperationLog.Create("Create", $"Claimed weapon {weapon.Codename}, with SN {weapon.SerialNumber}", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);

        await _unitOfWork.CompleteAsync();

        return weapon.Id;
    }

    public async Task DeleteAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");
        }

        if (weapon.Status == WeaponStatus.Deployed)
        {
            throw new InvalidOperationException("Cannot delete weapon because it is currently issued to a soldier. Return it to storage first.");
        }

        _unitOfWork.Weapons.Delete(weapon);

        var log = OperationLog.Create("Delete", $"Deleted weapon {weapon.Codename}, with SN {weapon.SerialNumber}", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateDetailsAsync(UpdateWeaponDetailsRequest request)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.Id);

        if (weapon is null)
        {
            throw new KeyNotFoundException($"Weapon with ID {request.Id} not found.");
        }

        bool hasChanges = false;
        var logDetails = new List<string>();

        if (request.Codename != weapon.Codename)
        {
            string oldCodeName = weapon.Codename;
            weapon.ChangeCodeName(request.Codename);
            logDetails.Add($"Codename: '{oldCodeName}' -> '{request.Codename}'");
            hasChanges = true;
        }

        if (request.SerialNumber != weapon.SerialNumber)
        {
            string oldSerialNumber = weapon.SerialNumber;
            weapon.CorrectSerialNumber(request.SerialNumber);
            logDetails.Add($"Serial Number: '{oldSerialNumber}' -> '{request.SerialNumber}'");
            hasChanges = true;
        }

        if (request.Caliber != weapon.Caliber)
        {
            string oldCaliber = weapon.Caliber;
            weapon.CorrectCaliber(request.Caliber);
            logDetails.Add($"Caliber: '{oldCaliber}' -> '{request.Caliber}'");
            hasChanges = true;
        }

        if (!hasChanges)
        {
            return;
        }

        _unitOfWork.Weapons.Update(weapon);

        var log = OperationLog.Create("Update", $"Updated details: {string.Join(", ", logDetails)}", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();

    }

    public async Task IssueWeaponAsync(Guid weaponId, Guid soldierId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");
        }

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId);

        if (soldier is null)
        {
            throw new KeyNotFoundException($"Soldier with ID {soldierId} not found.");
        }

        weapon.IssueTo(soldier.Id);

        _unitOfWork.Weapons.Update(weapon);

        var log = OperationLog.Create("Issue", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber} \n - Has been issued to a soldier with {soldierId} ID", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task ReturnToStorageAsync(Guid weaponId, int roundsFired)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");
        }

        weapon.ApplyWear(roundsFired);

        weapon.ReturnToStorage();

        _unitOfWork.Weapons.Update(weapon);

        var log = OperationLog.Create("Return", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been returned to storage with {weapon.Condition} condition", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task SendToMaintenanceAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");
        }

        weapon.SendToMaintenance();

        _unitOfWork.Weapons.Update(weapon);

        var log = OperationLog.Create("Maintenance", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been sent to maintenance", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);

        await _unitOfWork.CompleteAsync();
    }

    public async Task ReportMissingAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");
        }

        weapon.MarkAsMissing();

        _unitOfWork.Weapons.Update(weapon);

        var log = OperationLog.Create("Missing", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been marked as {weapon.Status}", weapon.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task<WeaponResponse?> GetWeaponByIdAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            return null;
        }

        return MapToResponse(weapon);
    }

    public async Task<List<WeaponResponse>> GetAllAsync()
    {
        var weapons = await _unitOfWork.Weapons.GetAllAsync();

        return weapons.Select(MapToResponse).ToList();
    }

    private static WeaponResponse MapToResponse(Weapon w)
    {
        return new WeaponResponse(
            w.Id,
            w.Codename,
            w.SerialNumber,
            w.Caliber,
            w.Type.ToString(),
            w.Status.ToString(),
            w.Condition,
            w.IssuedToSoldierId
        );
    }
}