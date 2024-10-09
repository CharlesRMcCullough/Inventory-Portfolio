using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IMakeIntegration
{
    Task<IEnumerable<MakeListViewModel>> GetMakesAsync();
    Task<MakeListViewModel> GetMakeByIdAsync(int id);
    Task<IEnumerable<MakeListViewModel>> GetMakesByCategoryIdAsync(int id);
    Task<IEnumerable<DropdownViewModel>> GetMakesForDropdownsAsync(int categoryId = 0);
    Task<MakeListViewModel> CreateMakeAsync(MakeListViewModel makeToAdd);
    Task<MakeListViewModel> UpdateMakeAsync(MakeListViewModel updatedMake);
    Task DeleteMakeAsync(int id);
}