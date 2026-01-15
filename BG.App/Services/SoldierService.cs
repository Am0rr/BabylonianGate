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

    public async Task<(Guid? Id, string Error)> CreateAsync(CreateSoldierRequest request)
    {
        if (!Enum.TryParse<SoldierRank>(request.Rank, true, out var rank))
        {
            return (null, $"Invalid Rank value: '{request.Rank}'. Allowed values: {string.Join(", ", Enum.GetNames(typeof(SoldierRank)))}");
        }

        var (soldier, error) = Soldier.Create(
            request.FirstName,
            request.LastName,
            rank
        );

        if (soldier is null)
        {
            return (null, error);
        }

        await _unitOfWork.Soldiers.AddAsync(soldier);

        var (log, _) = OperationLog.Create("Create", $"Recruited soldier {soldier.LastName}", soldier.Id);

        if (log != null)
        {
            await _unitOfWork.Logs.AddAsync(log);
        }

        await _unitOfWork.CompleteAsync();

        return (soldier.Id, string.Empty);
    }

    public async Task<string> UpdateAsync(UpdateSoldierRequest request)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.Id);

        if (soldier is null)
        {
            return "Soldier not found";
        }

        if (!Enum.TryParse<SoldierRank>(request.Rank, true, out var rank))
        {
            return $"Invalid Rank value: '{request.Rank}'. Allowed values: {string.Join(", ", Enum.GetNames(typeof(SoldierRank)))}";
        }

        bool hasChanges = false;
        var logDetails = new List<string>();

        try
        {
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
                return string.Empty;
            }

            _unitOfWork.Soldiers.Update(soldier);

            var (log, _) = OperationLog.Create("Update", $"Updated soldier {soldier.LastName}. Changes: {string.Join(", ", logDetails)}", soldier.Id);

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

    public async Task<string> DeleteAsync(Guid id)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(id);

        if (soldier is null)
        {
            return "Soldier not found";
        }

        var hasWeapon = await _unitOfWork.Weapons.HasAnyBySoldierIdAsync(id);

        if (hasWeapon)
        {
            return "Cannot delete soldier because they still have assigned weapons. Return weapons to storage first.";
        }

        try
        {
            _unitOfWork.Soldiers.Delete(soldier);

            var (log, _) = OperationLog.Create("Delete", $"Discharged soldier {soldier.LastName}", soldier.Id);

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

    public async Task<SoldierResponse?> GetSoldierByIdAsync(Guid id)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(id);

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