

using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface ILogService
{
    Task<(Guid? Id, string Error)> CreateAsync(CreateLogRequest request);
}