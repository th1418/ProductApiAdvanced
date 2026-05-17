using AutoMapper;
using ProductApiAdvanced.DTOs;
using ProductApiAdvanced.Entities;

namespace ProductApiAdvanced;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateDto, Product>();
    }
}