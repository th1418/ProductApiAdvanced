using System.Collections.Concurrent;
using ProductApiAdvanced.Entities;
using ProductApiAdvanced.Interfaces;

namespace ProductApiAdvanced.Repositories;

public class InMemoryRepository : IRepository<Product>
{
    private readonly ConcurrentDictionary<string, Product> _storage = new();

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.AsEnumerable());
    }

    public Task<Product?> GetByIdAsync(string id)
    {
        _storage.TryGetValue(id, out var product);

        return Task.FromResult(product);
    }

    public Task CreateAsync(Product product)
    {
        _storage[product.Id] = product;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        _storage.TryRemove(id, out _);

        return Task.CompletedTask;
    }
}
