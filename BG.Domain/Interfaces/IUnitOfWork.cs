using BG.Domain.Entities;

namespace BG.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IWeaponRepository Weapons { get; }
    IAmmoRepository Crates { get; }
    ILogRepository Logs { get; }
    ISoldierRepository Soldiers { get; }

    Task<int> CompleteAsync();
}