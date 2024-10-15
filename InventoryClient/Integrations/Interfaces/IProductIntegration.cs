using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IProductIntegration
{
    Task<IEnumerable<ProductListViewModel>> GetProductsAsync();
    Task<IEnumerable<DropdownViewModel>> GetProductsForDropdownsAsync();
    Task<ProductListViewModel> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductListViewModel>> GetProductsByCategoryIdAsync(int id);
    Task<ProductListViewModel> UpdateProductAsync(ProductListViewModel productToUpdate);
    Task<ProductListViewModel> CreateProductAsync(ProductListViewModel productToAdd);
    Task DeleteProductAsync(int id);
}