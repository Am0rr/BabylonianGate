using BG.Domain.Common;
using BG.Domain.Entities;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class SoldierRepository : ISoldierRepository
{
    private readonly BabylonianDbContext _context;

    public SoldierRepository(BabylonianDbContext context)
    {
        _context = context;
    }

    public async Task<List<Soldier>> GetAllAsync()
    {
        return await _context.Soldiers.AsNoTracking().ToListAsync();
    }

    public async Task<Guid> AddAsync(Soldier item)
    {
        await _context.AddAsync(item);

        return item.Id;
    }

    public async Task<Guid> DeleteAsync(Soldier item)
    {
        _context.Remove(item);

        return item.Id;
    }

    public async Task<Guid> UpdateAsync(Soldier item)
    {
        _context.Update(item);

        return item.Id;
    }
}