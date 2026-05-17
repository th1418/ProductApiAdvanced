using ProductApiAdvanced.Entities;
using ProductApiAdvanced.DTOs;

namespace ProductApiAdvanced.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(string id);

    Task<Product> CreateAsync(ProductCreateDto dto);

    Task<bool> DeleteAsync(string id);

    Task<List<Product>> GetPagedAsync(
    int page,
    int pageSize);
}
