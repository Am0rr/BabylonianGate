using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.Domain.Interfaces;

public interface IAmmoRepository : IRepository<AmmoCrate>
{
    Task<Guid> DeleteAsync(AmmoCrate item);
    Task<Guid> UpdateAsync(AmmoCrate item);
}