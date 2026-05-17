using ProductApiAdvanced.Entities;
using ProductApiAdvanced.DTOs;
using ProductApiAdvanced.Interfaces;
using ProductApiAdvanced.Repositories;
using ProductApiAdvanced.Services;
using ProductApiAdvanced.Configurations;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IRepository<Product>, InMemoryRepository>();

builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddScoped<IRepository<Product>, MongoProductRepository>();


builder.Services.AddScoped<IProductService, ProductService>();

// add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

/*
Console.WriteLine(
    $"Environment: {app.Environment.EnvironmentName}");
*/

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error" });
    }
});


app.MapGet("/products", async (IProductService service) =>
{
    return Results.Ok(await service.GetAllAsync());
});


app.MapGet("/products/{id}", async (string id, IProductService service) =>
{
    var product = await service.GetByIdAsync(id);

    return product is null
        ? Results.NotFound()
        : Results.Ok(product);
});


app.MapPost("/products", async (ProductCreateDto dto, IProductService service) =>
{
    // Old version of using try-catch in the endpoint, now handled by global error handling middleware
    // try
    // {
    //     var product = await service.CreateAsync(dto);

    //     return Results.Created($"/products/{product.Id}", product);
    // }
    // catch (ArgumentException ex)
    // {
    //     return Results.BadRequest(ex.Message);
    // }
    
     var product = await service.CreateAsync(dto);

     return Results.Created($"/products/{product.Id}", product);

});


app.MapDelete("/products/{id}", async (string id, IProductService service) =>
{
    var deleted = await service.DeleteAsync(id);

    return deleted
        ? Results.NoContent()
        : Results.NotFound();
});

app.MapGet("/products/paged",
    async (
        int page,
        int pageSize,
        IProductService service) =>
{
    return await service.GetPagedAsync(
        page,
        pageSize);
});

app.MapGet("/products/search",
    async (
        HttpContext context,
        IProductService service) =>
{
    var name = context.Request.Query["name"].ToString();
    
    if (string.IsNullOrEmpty(name))
        return Results.BadRequest("Name parameter is required");

    var products = await service.GetAllAsync();

    var filteredProducts = products.Where(p =>
        p.Name.Contains(
            name,
            StringComparison.OrdinalIgnoreCase));

    return Results.Ok(filteredProducts);
});

app.Run();
public partial class Program
{
}