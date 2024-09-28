using API.DTOs;


namespace API.Logic.Interfaces;

public interface IProductLogic
{
    Task <List<ProductDto>?> GetProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<List<DropdownDto>?> GetProductsForDropdownAsync();
}