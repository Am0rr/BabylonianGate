using BG.Domain.Entities;
using BG.Domain.Enums;

namespace BG.Domain.Interfaces;

public interface ISoldierRepository : IRepository<Soldier>
{
    void Delete(Soldier item);
    void Update(Soldier item);
}