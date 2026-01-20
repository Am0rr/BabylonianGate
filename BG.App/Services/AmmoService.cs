using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Interfaces;
using BG.Domain.Enums;
using BG.Domain.Entities;

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
        if (!Enum.TryParse<AmmoType>(request.Type, out var type))
        {
            return (null, $"Invalid Type value: '{request.Type}'. Allowed values: {string.Join(", ", Enum.GetNames(typeof(AmmoType)))}");
        }

        var (crate, error) = AmmoCrate.Create(
            request.LotNumber,
            request.Caliber,
            request.Quantity,
            type
        );

        if (crate is null)
        {
            return (null, error);
        }

        await _unitOfWork.Crates.AddAsync(crate);

        var (log, _) = OperationLog.Create("Create", $"Registered crate Lot #{crate.LotNumber} ({crate.Caliber}, {type})", crate.Id);

        if (log != null)
        {
            await _unitOfWork.Logs.AddAsync(log);
        }

        await _unitOfWork.CompleteAsync();

        return (crate.Id, string.Empty);
    }

    public async Task<string> DeleteAsync(Guid crateId)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId);

        if (crate is null)
        {
            return "Crate is not found";
        }

        if (crate.Quantity > 0)
        {
            return "Cannot delete crate containing ammo. Issue or empty it first.";
        }

        try
        {
            _unitOfWork.Crates.Delete(crate);

            var (log, _) = OperationLog.Create("Delete", $"Deleted crate Lot #{crate.LotNumber} ({crate.Caliber})", crate.Id);

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

    public async Task<string> UpdateDetailsAsync(UpdateAmmoDetailsRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.Id);

        if (crate is null)
        {
            return "Crate is not found";
        }

        if (!Enum.TryParse<AmmoType>(request.Type, out var type))
        {
            return $"Invalid Type value: '{request.Type}'. Allowed values: {string.Join(", ", Enum.GetNames(typeof(AmmoType)))}";
        }

        bool hasChanges = false;
        var logDetails = new List<string>();

        try
        {
            if (request.LotNumber != crate.LotNumber)
            {
                string oldLotNumber = crate.LotNumber;
                crate.CorrectLotNumber(request.LotNumber);
                logDetails.Add($"Lot: '{oldLotNumber}' -> '{request.LotNumber}'");
                hasChanges = true;
            }

            if (request.Caliber != crate.Caliber)
            {
                string oldCal = crate.Caliber;
                crate.CorrectCaliber(request.Caliber);
                logDetails.Add($"Caliber: '{oldCal}' -> '{request.Caliber}'");
                hasChanges = true;
            }

            if (crate.Type != type)
            {
                string oldType = crate.Type.ToString();
                crate.CorrectType(type);
                logDetails.Add($"Type: '{oldType}' -> '{request.Type}'");
                hasChanges = true;
            }

            if (!hasChanges)
            {
                return string.Empty;
            }

            _unitOfWork.Crates.Update(crate);

            var (log, _) = OperationLog.Create("Update", $"Corrected details: {string.Join(", ", logDetails)}", crate.Id);

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

    public async Task<string> IssueAmmoAsync(IssueAmmoRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId);

        if (crate is null)
        {
            return "Crate is not found";
        }

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId);

        if (soldier is null)
        {
            return "Soldier is not found";
        }

        try
        {
            crate.Issue(request.Amount);

            _unitOfWork.Crates.Update(crate);

            string logMessage = $"Issued {request.Amount} rounds ({crate.Type}) to {soldier.LastName} {soldier.FirstName}. " +
                            $"From Lot #{crate.LotNumber}. Remaining: {crate.Quantity}";

            var (log, _) = OperationLog.Create("Issue", logMessage, crate.Id);

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

    public async Task<string> RestockAsync(RestockAmmoRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId);

        if (crate is null)
        {
            return "Crate is not found";
        }

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId);

        if (soldier is null)
        {
            return "Soldier is not found";
        }

        try
        {
            crate.Restock(request.Amount);

            _unitOfWork.Crates.Update(crate);

            string logMessage = $"Restocked {request.Amount} rounds ({crate.Type}) from {soldier.LastName} {soldier.FirstName}. " +
                            $"In Lot #{crate.LotNumber}. Remaining: {crate.Quantity}";

            var (log, _) = OperationLog.Create("Restock", logMessage, crate.Id);

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

    public async Task<string> AuditInventoryAsync(Guid crateId, int actualQuantity)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId);

        if (crate is null)
        {
            return "Crate is not found";
        }

        try
        {
            int diff = actualQuantity - crate.Quantity;

            if (diff == 0) return string.Empty;

            crate.AdjustQuantity(actualQuantity);

            string diffSign = diff > 0 ? "+" : "";
            var (log, _) = OperationLog.Create("Audit", $"Inventory Check. Correction: {diffSign}{diff}. New Balance: {crate.Quantity}", crate.Id);

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

    public async Task<AmmoResponse?> GetCrateByIdAsync(Guid crateId)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId);

        if (crate is null)
        {
            return null;
        }

        return MapToResponse(crate);
    }

    public async Task<List<AmmoResponse>> GetAllAsync()
    {
        var crates = await _unitOfWork.Crates.GetAllAsync();

        return crates.Select(MapToResponse).ToList();
    }

    private static AmmoResponse MapToResponse(AmmoCrate a)
    {
        return new AmmoResponse(
            a.Id,
            a.LotNumber,
            a.Caliber,
            a.Quantity,
            a.Type.ToString()
        );
    }
}