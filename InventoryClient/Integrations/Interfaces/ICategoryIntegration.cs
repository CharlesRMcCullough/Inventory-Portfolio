using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface ICategoryIntegration
{
    Task<IEnumerable<CategoryListViewModel>> GetCategoriesAsync();
    Task<CategoryListViewModel> GetCategoryByIdAsync(int id);
    Task<CategoryListViewModel> CreateCategoryAsync(CategoryListViewModel categoryToAdd);
    Task<CategoryListViewModel> UpdateCategoryAsync(CategoryListViewModel updatedCategory);
    Task DeleteCategoryAsync(int id);
}