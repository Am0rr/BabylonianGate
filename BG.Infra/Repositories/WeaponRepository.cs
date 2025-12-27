using BG.Domain.Interfaces;
using BG.Domain.Entities;
using BG.Infra.Persistence;
using BG.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Repositories;

public class WeaponRepository : IWeaponRepository
{
    private readonly BabylonianDbContext _context;

    public WeaponRepository(BabylonianDbContext context)
    {
        _context = context;
    }

    public async Task<List<Weapon>> GetAllAsync()
    {
        return await _context.Weapons.AsNoTracking().ToListAsync();
    }

    public async Task<Guid> AddAsync(Weapon item)
    {
        await _context.Weapons.AddAsync(item);

        return item.Id;
    }

    public async Task<Guid> DeleteAsync(Weapon item)
    {
        _context.Weapons.Remove(item);

        return item.Id;
    }

    public async Task<Guid> UpdateAsync(Weapon item)
    {
        _context.Weapons.Update(item);

        return item.Id;
    }

    public async Task<Guid> ChangeStatusAsync(Weapon item)
    {
        _context.Weapons.Update(item);

        return item.Id;
    }
}