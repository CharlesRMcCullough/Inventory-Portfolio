using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IProductIntegration
{
    Task<IEnumerable<ProductListViewModel>> GetProductsAsync();
}