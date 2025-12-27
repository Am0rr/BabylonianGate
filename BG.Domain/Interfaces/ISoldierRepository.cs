using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.Domain.Interfaces;

public interface ISoldierRepository : IRepository<Soldier>
{
    Task<Guid> DeleteAsync(Soldier item);
    Task<Guid> UpdateAsync(Soldier item);
}