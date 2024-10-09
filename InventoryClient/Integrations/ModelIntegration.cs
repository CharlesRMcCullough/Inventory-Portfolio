using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class ModelIntegration : IModelIntegration
{
    private const string ApiBase = "/api/models";
    private const string ApiUrl = "http://localhost:7001";
    
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri(ApiUrl)
    };
    
    public async Task<IEnumerable<ModelListViewModel>> GetModelsAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase);
        var returnModels = new List<ModelListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnModels = JsonConvert.DeserializeObject<List<ModelListViewModel>>(data);
        }

        if (returnModels == null)
            return new List<ModelListViewModel>();

        return returnModels;
    }

    public async Task<ModelListViewModel> GetModelByIdAsync(int id)
    {
        var response = await HttpClient.GetAsync(ApiBase + $"/{id}");
        var returnModel = new ModelListViewModel();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnModel = JsonConvert.DeserializeObject<ModelListViewModel>(data);
        }

        if (returnModel == null)
            return new ModelListViewModel();

        return returnModel;
    }
    
    public async Task<IEnumerable<ModelListViewModel>> GetModelsByMakeIdAsync(int id)
    {
        var response = await HttpClient.GetAsync(ApiBase + $"/byMake/{id}");
        var returnModels = new List<ModelListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnModels = JsonConvert.DeserializeObject<List<ModelListViewModel>>(data);
        }

        if (returnModels == null)
            return new List<ModelListViewModel>();

        return returnModels;
    }
    
    public async Task<IEnumerable<DropdownViewModel>> GetModelsForDropdownsAsync(int makeId = 0)
    {
        var response = await HttpClient.GetAsync($"{ApiBase}/dropdowns/{makeId}");
        var returnModels = new List<DropdownViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnModels = JsonConvert.DeserializeObject<List<DropdownViewModel>>(data);
        }

        if (returnModels == null)
            return new List<DropdownViewModel>();

        return returnModels;
    }
    
    public async Task<ModelListViewModel> UpdateModelAsync(ModelListViewModel updatedModel)
    {
        var modelDto = new ModelDto()
        {
            Id = updatedModel.Id,
            Name = updatedModel.Name,
            Description = updatedModel.Description,
            Status = updatedModel.Status,
            MakeId = updatedModel.MakeId
        };
        
        using StringContent jsonContent = 
            new StringContent(JsonConvert.SerializeObject(modelDto), Encoding.UTF8, "application/json");
        
        var response = await HttpClient.PutAsync(ApiBase, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnModel = JsonConvert.DeserializeObject<ModelListViewModel>(data);
        
        return returnModel ?? new ModelListViewModel();
    }
    
    public async Task<ModelListViewModel> CreateModelAsync(ModelListViewModel modelToAdd)
    {
        var modelDto = new ModelDto()
        {
            Id = modelToAdd.Id,
            Name = modelToAdd.Name,
            Description = modelToAdd.Description,
            Status = modelToAdd.Status,
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(modelDto), Encoding.UTF8, "application/json");
        
        var response = await HttpClient.PostAsync(ApiBase, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnModel = JsonConvert.DeserializeObject<ModelListViewModel>(data);
        
        return returnModel ?? new ModelListViewModel();
    }

    public async Task DeleteModelAsync(int id)
    {
        await HttpClient.DeleteAsync(ApiBase + $"/{id}");
    }
}