using InventoryClient.ViewModels;

namespace InventoryClient.Integrations.Interfaces;

public interface IModelIntegration
{
    Task<IEnumerable<ModelListViewModel>> GetModelsAsync();
    Task<ModelListViewModel> GetModelByIdAsync(int id);
    Task<ModelListViewModel> CreateModelAsync(ModelListViewModel modelToAdd);
    Task<ModelListViewModel> UpdateModelAsync(ModelListViewModel updatedModel);
    Task DeleteModelAsync(int id);
}