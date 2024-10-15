using API.DTOs;


namespace API.Logic.Interfaces;

public interface IProductLogic
{
    Task <List<ProductDto>> GetProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<List<ProductDto>> GetProductsByCategoryAsync(int id);
    Task<List<DropdownDto>> GetProductsForDropdownAsync();
    Task<ProductDto> CreateProductAsync(ProductDto productDto);
    Task<ProductDto> UpdateProductAsync(ProductDto productDto);
    Task DeleteProductsAsync(int id);
}