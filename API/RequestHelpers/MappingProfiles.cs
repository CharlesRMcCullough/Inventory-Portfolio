using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
            .IncludeMembers(_ => _.Category);
        CreateMap<Category, ProductDto>();
        CreateMap<Make, ProductDto>();
        CreateMap<Model, ProductDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<MakeDto, Make>();
        CreateMap<Make, MakeDto>();
        CreateMap<ModelDto, Model>();
        CreateMap<Model, ModelDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<Product, ProductDto>();

    }
    
}