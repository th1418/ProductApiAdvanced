using Xunit;
using Moq;
using AutoMapper;
using ProductApiAdvanced.Services;
using ProductApiAdvanced.Interfaces;
using ProductApiAdvanced.Entities;
using ProductApiAdvanced.DTOs;

public class ProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repoMock;
    private readonly IMapper _mapper;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IRepository<Product>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductCreateDto, Product>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNameIsEmpty()
    {
        // Arrange
        var service = new ProductService(_repoMock.Object, _mapper, null!);

        var dto = new ProductCreateDto
        {
            Name = "",
            Price = 10
        };

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(dto));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProductNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync("1"))
                .ReturnsAsync((Product?)null);

        var service = new ProductService(_repoMock.Object, _mapper, null!);

        // Act
        var result = await service.DeleteAsync("1");

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPage()
    {
        var products = new List<Product>
    {
        new Product { Id = "1", Name = "A", Price = 1 },
        new Product { Id = "2", Name = "B", Price = 2 },
        new Product { Id = "3", Name = "C", Price = 3 },
        new Product { Id = "4", Name = "D", Price = 4 }
    };

        _repoMock.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(products);

        var service = new ProductService(_repoMock.Object, _mapper, null!);

        var page2 = await service.GetPagedAsync(2, 2);

        Assert.Equal(2, page2.Count);
        Assert.Equal("C", page2[0].Name);
        Assert.Equal("D", page2[1].Name);
    }

}