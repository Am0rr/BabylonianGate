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

        try
        {
            if (soldier.FirstName != request.FirstName || soldier.LastName != request.LastName)
            {
                soldier.UpdateName(request.FirstName, request.LastName);
            }

            if (soldier.Rank != rank)
            {
                soldier.UpdateRank(rank);
            }

            _unitOfWork.Soldiers.Update(soldier);

            var (log, _) = OperationLog.Create("Update", $"Updated soldier {soldier.LastName}", soldier.Id);

            if (log != null)
            {
                await _unitOfWork.Logs.AddAsync(log);
            }

            await _unitOfWork.CompleteAsync();

            return string.Empty;
        }
        catch (ArgumentException ex)
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

        catch (ArgumentException ex)
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

        return new SoldierResponse(
            soldier.Id,
            soldier.FirstName,
            soldier.LastName,
            soldier.Rank.ToString()
        );
    }

    public async Task<List<SoldierResponse>> GetAllAsync()
    {
        var soldiers = await _unitOfWork.Soldiers.GetAllAsync();

        return soldiers.Select(s => new SoldierResponse(
            s.Id,
            s.FirstName,
            s.LastName,
            s.Rank.ToString()
        )).ToList();
    }
}