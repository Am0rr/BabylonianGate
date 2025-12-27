

namespace BG.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<Guid> AddAsync(T item);
}