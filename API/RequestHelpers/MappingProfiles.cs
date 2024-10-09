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
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId))
            .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

    }
    
}