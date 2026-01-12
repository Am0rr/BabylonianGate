

using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface ILogService
{
    Task<LogResponse?> GetLogByIdAsync(Guid id);
    Task<List<LogResponse>> GetHistoryByEntityIdAsync(Guid entityId);
    Task<List<LogResponse>> GetRecentLogsAsync(int count);
}