using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.Domain.Interfaces;

public interface IAmmoRepository : IRepository<AmmoCrate>
{
    void Delete(AmmoCrate item);
    void Update(AmmoCrate item);
}