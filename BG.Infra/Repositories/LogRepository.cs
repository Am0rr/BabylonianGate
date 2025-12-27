

using BG.Domain.Entities;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class LogRepository : ILogRepository
{
    private readonly BabylonianDbContext _context;

    public LogRepository(BabylonianDbContext context)
    {
        _context = context;
    }

    public async Task<List<OperationLog>> GetAllAsync()
    {
        return await _context.OperationLogs.AsNoTracking().ToListAsync();
    }

    public async Task<Guid> AddAsync(OperationLog item)
    {
        await _context.AddAsync(item);

        return item.Id;
    }
}