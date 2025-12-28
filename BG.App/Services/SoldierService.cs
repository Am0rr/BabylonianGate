
using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Interfaces;

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
        throw new NotImplementedException();
    }

    public async Task<string> UpdateAsync(UpdateSoldierRequest request)
    {
        throw new NotImplementedException();
    }
}