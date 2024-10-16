using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class ModelIntegration : IModelIntegration
{

    private readonly HttpClient _httpClient;

    public ModelIntegration(IOptions<ApiSettings> apiSettings)
    {
        var settings = apiSettings.Value;
        _httpClient = new HttpClient { BaseAddress = new Uri(settings.ApiUrl + settings.ModelApiBase) };
    }
    public async Task<IEnumerable<ModelListViewModel>> GetModelsAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
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
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");
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
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/byMake/{id}");
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
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/dropdowns/{makeId}");
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
        
        var response = await _httpClient.PutAsync(_httpClient.BaseAddress, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnModel = JsonConvert.DeserializeObject<ModelListViewModel>(data);
        
        return returnModel ?? new ModelListViewModel();
    }
    
    public async Task<ModelListViewModel> CreateModelAsync(ModelListViewModel modelToAdd)
    {
        var modelDto = new ModelDto()
        {
            Id = modelToAdd.Id,
            MakeId = modelToAdd.MakeId,
            Name = modelToAdd.Name,
            Description = modelToAdd.Description,
            Status = modelToAdd.Status,
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(modelDto), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnModel = JsonConvert.DeserializeObject<ModelListViewModel>(data);
        
        return returnModel ?? new ModelListViewModel();
    }

    public async Task DeleteModelAsync(int id)
    {
        await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}");
    }
}