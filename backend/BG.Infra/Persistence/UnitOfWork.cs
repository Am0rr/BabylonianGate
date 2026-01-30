
using BG.Infra.Repositories;
using BG.Domain.Interfaces;
using BG.Domain.Entities;

namespace BG.Infra.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BabylonianDbContext _context;

    public IWeaponRepository Weapons { get; private set; }
    public IAmmoRepository Crates { get; private set; }
    public ILogRepository Logs { get; private set; }
    public ISoldierRepository Soldiers { get; private set; }

    public UnitOfWork(BabylonianDbContext context)
    {
        _context = context;

        Weapons = new WeaponRepository(_context);
        Crates = new AmmoRepository(_context);
        Logs = new LogRepository(_context);
        Soldiers = new SoldierRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}