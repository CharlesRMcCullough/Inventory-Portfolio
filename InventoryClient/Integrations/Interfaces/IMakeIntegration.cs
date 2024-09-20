using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IMakeIntegration
{
    Task<IEnumerable<MakeListViewModel>> GetMakesAsync();
    Task<MakeListViewModel> GetMakeByIdAsync(int id);
    Task<MakeListViewModel> CreateMakeAsync(MakeListViewModel makeToAdd);
    Task<MakeListViewModel> UpdateMakeAsync(MakeListViewModel updatedMake);
    Task DeleteMakeAsync(int id);
}