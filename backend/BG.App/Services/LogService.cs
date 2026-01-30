using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork _unitOfWork;

    public LogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LogResponse?> GetLogByIdAsync(Guid id)
    {
        var log = await _unitOfWork.Logs.GetByIdAsync(id);

        if (log is null)
        {
            return null;
        }

        return MapToResponse(log);
    }

    public async Task<List<LogResponse>> GetHistoryByEntityIdAsync(Guid entityId)
    {
        var logs = await _unitOfWork.Logs.FindAsync(l => l.RelatedEntityId == entityId);

        return logs.OrderByDescending(x => x.CreatedAt).Select(MapToResponse).ToList();
    }

    public async Task<List<LogResponse>> GetRecentLogsAsync(int count = 100)
    {
        var logs = await _unitOfWork.Logs.GetRecentAsync(count);

        return logs.Select(MapToResponse).ToList();
    }

    private static LogResponse MapToResponse(OperationLog o)
    {
        return new LogResponse(
            o.Id,
            o.Action,
            o.Details,
            o.CreatedAt,
            o.RelatedEntityId,
            o.OperatorId
        );
    }
}