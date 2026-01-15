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

    public async Task<string> DeleteAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            return "Weapon not found";
        }

        if (weapon.Status == WeaponStatus.Deployed)
        {
            return "Cannot delete weapon because it is currently issued to a soldier. Return it to storage first.";
        }

        try
        {
            _unitOfWork.Weapons.Delete(weapon);

            var (log, _) = OperationLog.Create("Delete", $"Deleted weapon {weapon.Codename}, with SN {weapon.SerialNumber}", weapon.Id);

            if (log != null)
            {
                await _unitOfWork.Logs.AddAsync(log);
            }

            await _unitOfWork.CompleteAsync();

            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> UpdateDetailsAsync(UpdateWeaponDetailsRequest request)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.Id);

        if (weapon is null)
        {
            return "Weapon not found";
        }

        bool hasChanges = false;
        var logDetails = new List<string>();

        try
        {
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
                return string.Empty;
            }

            _unitOfWork.Weapons.Update(weapon);

            var (log, _) = OperationLog.Create("Update", $"Updated details: {string.Join(", ", logDetails)}", weapon.Id);

            if (log != null)
            {
                await _unitOfWork.Logs.AddAsync(log);
            }

            await _unitOfWork.CompleteAsync();

            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> IssueWeaponAsync(Guid weaponId, Guid soldierId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            return "Weapon not found";
        }

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId);

        if (soldier is null)
        {
            return "Soldier not found";
        }

        try
        {
            weapon.IssueTo(soldier.Id);

            _unitOfWork.Weapons.Update(weapon);

            var (log, _) = OperationLog.Create("Issue", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber} \n - Has been issued to a soldier with {soldierId} ID", weapon.Id);

            if (log != null)
            {
                await _unitOfWork.Logs.AddAsync(log);
            }

            await _unitOfWork.CompleteAsync();

            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> ReturnToStorageAsync(Guid weaponId, int roundsFired)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            return "Weapon not found";
        }

        try
        {
            weapon.ApplyWear(roundsFired);

            weapon.ReturnToStorage();

            _unitOfWork.Weapons.Update(weapon);

            var (log, _) = OperationLog.Create("Return", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been returned to storage with {weapon.Condition} condition", weapon.Id);

            if (log != null)
            {
                await _unitOfWork.Logs.AddAsync(log);
            }

            await _unitOfWork.CompleteAsync();

            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> SendToMaintenanceAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            return "Weapon not found";
        }

        try
        {
            weapon.SendToMaintenance();

            _unitOfWork.Weapons.Update(weapon);

            var (log, _) = OperationLog.Create("Maintenance", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been sent to maintenance", weapon.Id);

            if (log != null)
            {
                await _unitOfWork.Logs.AddAsync(log);
            }

            await _unitOfWork.CompleteAsync();

            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> ReportMissingAsync(Guid weaponId)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId);

        if (weapon is null)
        {
            return "Weapon not found";
        }

        weapon.MarkAsMissing();

        _unitOfWork.Weapons.Update(weapon);

        var (log, _) = OperationLog.Create("Missing", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been marked as {weapon.Status}", weapon.Id);

        if (log != null)
        {
            await _unitOfWork.Logs.AddAsync(log);
        }

        await _unitOfWork.CompleteAsync();

        return string.Empty;
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