using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.Domain.Interfaces;

public interface IWeaponRepository : IRepository<Weapon>
{
    Task<Guid> DeleteAsync(Weapon item);
    Task<Guid> UpdateAsync(Weapon item);
    Task<Guid> ChangeStatusAsync(Weapon item);
}