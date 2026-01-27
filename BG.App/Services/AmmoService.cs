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

    public async Task<Guid> CreateAsync(CreateAmmoRequest request)
    {
        var type = Enum.Parse<AmmoType>(request.Type, ignoreCase: true);

        var crate = AmmoCrate.Create(
            request.LotNumber,
            request.Caliber,
            request.Quantity,
            type
        );

        await _unitOfWork.Crates.AddAsync(crate);

        var log = OperationLog.Create("Create", $"Registered crate Lot #{crate.LotNumber} ({crate.Caliber}, {type})", crate.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();

        return crate.Id;
    }

    public async Task DeleteAsync(Guid crateId)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId);

        if (crate is null)
        {
            throw new KeyNotFoundException($"Crate with ID {crateId} not found.");
        }

        if (crate.Quantity > 0)
        {
            throw new InvalidOperationException("Cannot delete crate containing ammo. Issue or empty it first.");
        }

        _unitOfWork.Crates.Delete(crate);

        var log = OperationLog.Create("Delete", $"Deleted crate Lot #{crate.LotNumber} ({crate.Caliber})", crate.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateDetailsAsync(UpdateAmmoDetailsRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.Id);

        if (crate is null)
        {
            throw new KeyNotFoundException($"Crate with ID {request.Id} not found.");
        }

        var type = Enum.Parse<AmmoType>(request.Type, ignoreCase: true);

        bool hasChanges = false;
        var logDetails = new List<string>();

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
            return;
        }

        _unitOfWork.Crates.Update(crate);

        var log = OperationLog.Create("Update", $"Corrected details: {string.Join(", ", logDetails)}", crate.Id);
        await _unitOfWork.Logs.AddAsync(log);

        await _unitOfWork.CompleteAsync();
    }

    public async Task IssueAmmoAsync(IssueAmmoRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId);

        if (crate is null)
        {
            throw new KeyNotFoundException($"Crate with ID {request.CrateId} not found.");
        }

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId);

        if (soldier is null)
        {
            throw new KeyNotFoundException($"Soldier with ID {request.SoldierId} not found.");
        }

        crate.Issue(request.Amount);

        _unitOfWork.Crates.Update(crate);

        string logMessage = $"Issued {request.Amount} rounds ({crate.Type}) to {soldier.LastName} {soldier.FirstName}. " +
                        $"From Lot #{crate.LotNumber}. Remaining: {crate.Quantity}";

        var log = OperationLog.Create("Issue", logMessage, crate.Id);

        await _unitOfWork.Logs.AddAsync(log);

        await _unitOfWork.CompleteAsync();
    }

    public async Task RestockAsync(RestockAmmoRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId);

        if (crate is null)
        {
            throw new KeyNotFoundException($"Crate with ID {request.CrateId} not found.");
        }

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId);

        if (soldier is null)
        {
            throw new KeyNotFoundException($"Soldier with ID {request.SoldierId} not found.");
        }

        crate.Restock(request.Amount);

        _unitOfWork.Crates.Update(crate);

        string logMessage = $"Restocked {request.Amount} rounds ({crate.Type}) from {soldier.LastName} {soldier.FirstName}. " +
                        $"In Lot #{crate.LotNumber}. Remaining: {crate.Quantity}";

        var log = OperationLog.Create("Restock", logMessage, crate.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task AuditInventoryAsync(AuditAmmoInventoryRequest request)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId);

        if (crate is null)
        {
            throw new KeyNotFoundException($"Crate with ID {request.CrateId} not found.");
        }

        int diff = request.ActualQuantity - crate.Quantity;

        if (diff == 0) return;

        crate.AdjustQuantity(request.ActualQuantity);

        _unitOfWork.Crates.Update(crate);

        string diffSign = diff > 0 ? "+" : "";
        var log = OperationLog.Create("Audit", $"Inventory Check. Correction: {diffSign}{diff}. New Balance: {crate.Quantity}", crate.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
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