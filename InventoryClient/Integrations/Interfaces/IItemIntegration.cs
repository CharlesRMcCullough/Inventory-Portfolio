using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IItemIntegration
{
    Task<IReadOnlyList<ItemListViewModel>> GetItemsAsync();
    Task<IReadOnlyList<ItemListViewModel>> GetItemsByProductId(int productId);
    Task<ItemListViewModel> GetItemByItemId(int itemId);
}