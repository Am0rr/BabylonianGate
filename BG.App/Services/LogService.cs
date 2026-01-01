using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork _unitOfWork;

    public LogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(Guid? Id, string Error)> CreateAsync(CreateLogRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<LogResponse?> GetLogByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}