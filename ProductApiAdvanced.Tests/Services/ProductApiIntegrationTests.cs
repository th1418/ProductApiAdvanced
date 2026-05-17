using System.Net.Http.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProductApiAdvanced.Entities;
using ProductApiAdvanced.Interfaces;
using ProductApiAdvanced.Repositories;

namespace ProductApiAdvanced.Tests;

public class ProductApiIntegrationTests
{
    [Fact]
    public async Task GetProducts_ReturnsSuccess()
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IRepository<Product>));
                    if (descriptor != null)
                        services.Remove(descriptor!);

                    services.AddSingleton<IRepository<Product>, InMemoryRepository>();
                });
            });

        var client = factory.CreateClient();

        var response =
            await client.GetAsync("/products");

        response.EnsureSuccessStatusCode();
    }
}