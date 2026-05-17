using MongoDB.Driver;
using ProductApiAdvanced.Entities;
using ProductApiAdvanced.Interfaces;
using ProductApiAdvanced.Configurations;
using Microsoft.Extensions.Options;

namespace ProductApiAdvanced.Repositories;

public class MongoProductRepository : IRepository<Product>
{
    private readonly IMongoCollection<Product> _collection;

    public MongoProductRepository(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);

        var database = client.GetDatabase(settings.Value.DatabaseName);

        _collection = database.GetCollection<Product>(settings.Value.ProductCollection);

        // if (typeof(T) == typeof(Product))
        // {
        //     var productCollection =
        //         _collection as IMongoCollection<Product>;

        //     var indexKeys =
        //         Builders<Product>.IndexKeys
        //             .Ascending(p => p.Name);

        //     productCollection?.Indexes.CreateOne(
        //         new CreateIndexModel<Product>(indexKeys));
        // }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _collection
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product product)
    {
        await _collection.InsertOneAsync(product);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
