using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.Domain.Interfaces;

public interface IWeaponRepository : IRepository<Weapon>
{
    void Delete(Weapon item);
    void Update(Weapon item);
    Task<bool> HasAnyBySoldierIdAsync(Guid soldierId);
}