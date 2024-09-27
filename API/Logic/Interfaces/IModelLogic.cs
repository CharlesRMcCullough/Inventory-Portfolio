using API.DTOs;

namespace API.Logic.Interfaces;

public interface IModelLogic
{
    Task<List<ModelDto>?> GetModelsAsync();
    Task<ModelDto?> GetModelByIdAsync(int id);
    Task<List<DropdownDto>?> GetModelsForDropdownAsync();
    Task<List<ModelDto>?> GetModelsByMakeIdAsync(int makeId);
    Task<ModelDto?> CreateModelAsync(ModelDto modelDto);
    Task<ModelDto?> UpdateModelAsync(ModelDto modelDto);
    Task DeleteModelAsync(int id);
}