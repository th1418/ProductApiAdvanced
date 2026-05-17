namespace ProductApiAdvanced.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(string id);

    Task CreateAsync(T entity);

    Task DeleteAsync(string id);
}
