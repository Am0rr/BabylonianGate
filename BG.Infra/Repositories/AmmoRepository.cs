using BG.Domain.Entities;
using BG.Domain.Interfaces;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class AmmoRepository : IAmmoRepository
{
    private readonly BabylonianDbContext _context;

    public AmmoRepository(BabylonianDbContext context)
    {
        _context = context;
    }

    public async Task<List<AmmoCrate>> GetAllAsync()
    {
        return await _context.AmmoCrates.AsNoTracking().ToListAsync();
    }

    public async Task<Guid> AddAsync(AmmoCrate item)
    {
        await _context.AmmoCrates.AddAsync(item);

        return item.Id;
    }

    public async Task<Guid> DeleteAsync(AmmoCrate item)
    {
        _context.AmmoCrates.Remove(item);

        return item.Id;
    }

    public async Task<Guid> UpdateAsync(AmmoCrate item)
    {
        _context.AmmoCrates.Update(item);

        return item.Id;
    }

}