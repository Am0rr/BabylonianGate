using BG.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var log = await _logService.GetLogByIdAsync(id);
        return Ok(log);
    }

    [HttpGet("history/{entityId:guid}")]
    public async Task<IActionResult> GetHistoryByEntityId(Guid entityId)
    {
        var logs = await _logService.GetHistoryByEntityIdAsync(entityId);
        return Ok(logs);
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentLogs([FromQuery] int count = 100)
    {
        var logs = await _logService.GetRecentLogsAsync(count);
        return Ok(logs);
    }
}