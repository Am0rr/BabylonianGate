using BG.Domain.Entities;
using System.Linq.Expressions;

namespace BG.Domain.Interfaces;

public interface ILogRepository : IRepository<OperationLog>
{
    Task<List<OperationLog>> FindAsync(Expression<Func<OperationLog, bool>> predicate);
    Task<List<OperationLog>> GetRecentAsync(int count);
}