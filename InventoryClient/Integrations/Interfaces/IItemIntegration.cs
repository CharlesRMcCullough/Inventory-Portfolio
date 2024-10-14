using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IItemIntegration
{
    Task<IReadOnlyList<ItemListViewModel>> GetItemsAsync();
    Task<IReadOnlyList<ItemListViewModel>> GetItemsByProductId(int productId);
    Task<ItemListViewModel> GetItemById(int itemId);
    Task<ItemListViewModel> CreateItemAsync(ItemListViewModel itemToAdd);
    Task<ItemListViewModel> UpdateItemAsync(ItemListViewModel itemToUpdate);
    Task DeleteItemAsync(int id);
}
