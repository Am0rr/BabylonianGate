using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.App.Services;

public class SoldierService : ISoldierService
{
    private readonly IUnitOfWork _unitOfWork;

    public SoldierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAsync(CreateSoldierRequest request)
    {
        var rank = Enum.Parse<SoldierRank>(request.Rank, ignoreCase: true);

        var soldier = Soldier.Create(
            request.FirstName,
            request.LastName,
            rank
        );

        await _unitOfWork.Soldiers.AddAsync(soldier);

        var log = OperationLog.Create("Create", $"Recruited soldier {soldier.LastName}", soldier.Id);
        await _unitOfWork.Logs.AddAsync(log);

        await _unitOfWork.CompleteAsync();

        return soldier.Id;
    }

    public async Task UpdateAsync(UpdateSoldierRequest request)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.Id);

        if (soldier is null)
        {
            throw new KeyNotFoundException($"Soldier with ID {request.Id} not found.");
        }

        var rank = Enum.Parse<SoldierRank>(request.Rank, ignoreCase: true);

        bool hasChanges = false;
        var logDetails = new List<string>();

        if (soldier.FirstName != request.FirstName || soldier.LastName != request.LastName)
        {
            string oldFirstName = soldier.FirstName;
            string oldLastName = soldier.LastName;
            soldier.UpdateName(request.FirstName, request.LastName);
            logDetails.Add($"First and Last name: '{oldFirstName}' '{oldLastName}' -> '{request.FirstName}' '{request.LastName}'");
            hasChanges = true;
        }

        if (soldier.Rank != rank)
        {
            string oldRank = soldier.Rank.ToString();
            soldier.UpdateRank(rank);
            logDetails.Add($"Rank: '{oldRank}' -> '{request.Rank}'");
            hasChanges = true;
        }

        if (!hasChanges)
        {
            return;
        }

        _unitOfWork.Soldiers.Update(soldier);

        var log = OperationLog.Create("Update", $"Updated soldier {soldier.LastName}. Changes: {string.Join(", ", logDetails)}", soldier.Id);
        await _unitOfWork.Logs.AddAsync(log);


        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(Guid soldierId)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId);

        if (soldier is null)
        {
            throw new KeyNotFoundException($"Soldier with ID {soldierId} not found.");
        }

        var hasWeapon = await _unitOfWork.Weapons.HasAnyBySoldierIdAsync(soldierId);

        if (hasWeapon)
        {
            throw new InvalidOperationException("Cannot delete soldier because they still have assigned weapons. Return weapons to storage first.");
        }

        _unitOfWork.Soldiers.Delete(soldier);

        var log = OperationLog.Create("Delete", $"Discharged soldier {soldier.LastName}", soldier.Id);
        await _unitOfWork.Logs.AddAsync(log);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<SoldierResponse?> GetSoldierByIdAsync(Guid soldierId)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId);

        if (soldier is null)
        {
            return null;
        }

        return MapToResponse(soldier);
    }

    public async Task<List<SoldierResponse>> GetAllAsync()
    {
        var soldiers = await _unitOfWork.Soldiers.GetAllAsync();

        return soldiers.Select(MapToResponse).ToList();
    }

    private static SoldierResponse MapToResponse(Soldier s)
    {
        return new SoldierResponse(
            s.Id,
            s.FirstName,
            s.LastName,
            s.Rank.ToString()
        );
    }
}