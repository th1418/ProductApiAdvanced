using ProductApiAdvanced.Entities;
using ProductApiAdvanced.DTOs;
using ProductApiAdvanced.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ProductApiAdvanced.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IRepository<Product> repository, IMapper mapper, ILogger<ProductService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> CreateAsync(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name required");

        if (dto.Price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        var existingProducts = await _repository.GetAllAsync();

        if (existingProducts.Any(p =>
             string.Equals(p.Name?.Trim(), dto.Name?.Trim(),
                  StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException(
            "Product already exists");
        }

        // old useage of product creation
        /*
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name,
            Price = dto.Price
        };
        */

         // new useage of product creation
         var product = _mapper.Map<Product>(dto);
         //product.Id = Guid.NewGuid().ToString();
         //product.Name = dto.Name;
         //product.Price = dto.Price;


        await _repository.CreateAsync(product);

        _logger.LogInformation("Creating product {Name}", dto.Name);

        return product;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return false;

        await _repository.DeleteAsync(id);

        _logger.LogInformation("Deleting product {Name}", id);

        return true;
    }

    public async Task<List<Product>> GetPagedAsync(
        int page,
        int pageSize)
    {
        var products = await _repository.GetAllAsync();

        return products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

}
