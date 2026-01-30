using System.Linq.Expressions;
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
        throw new NotSupportedException("Fetching ALL logs is not allowed due to performance. Use GetRecentAsync instead.");
    }

    public async Task<OperationLog?> GetByIdAsync(Guid id)
    {
        return await _context.OperationLogs.FindAsync(id);
    }

    public async Task<Guid> AddAsync(OperationLog item)
    {
        await _context.AddAsync(item);

        return item.Id;
    }

    public async Task<List<OperationLog>> FindAsync(Expression<Func<OperationLog, bool>> predicate)
    {
        return await _context.OperationLogs.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<List<OperationLog>> GetRecentAsync(int count)
    {
        return await _context.OperationLogs.AsNoTracking().OrderByDescending(x => x.CreatedAt).Take(count).ToListAsync();
    }
}