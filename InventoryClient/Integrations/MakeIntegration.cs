using System;
using System.Text;
using API.DTOs;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public interface IMakeIntegration
{
    Task<IEnumerable<MakeListViewModel>> GetMakesAsync();
    Task<MakeListViewModel> GetMakeByIdAsync(int id);
    Task<MakeListViewModel> CreateMakeAsync(MakeListViewModel makeToAdd);
    Task<MakeListViewModel> UpdateMakeAsync(MakeListViewModel updatedMake);
    Task DeleteMakeAsync(int id);
}


public class MakeIntegration : IMakeIntegration
{
        private const string ApiBase = "/api/makes";
        private const string ApiUrl = "http://localhost:7001";

     private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(ApiUrl)
    };

    public async Task<IEnumerable<MakeListViewModel>> GetMakesAsync()
    {
        var response = await _httpClient.GetAsync(ApiBase);
        var returnCategories = new List<MakeListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnCategories = JsonConvert.DeserializeObject<List<MakeListViewModel>>(data);
        }

        if (returnCategories == null)
            return new List<MakeListViewModel>();

        return returnCategories;
    }

    public async Task<MakeListViewModel> GetMakeByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync(ApiBase + $"/{id}");
        var returnMake = new MakeListViewModel();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnMake = JsonConvert.DeserializeObject<MakeListViewModel>(data);
        }

        if (returnMake == null)
            return new MakeListViewModel();

        return returnMake;
    }

    public async Task<MakeListViewModel> UpdateMakeAsync(MakeListViewModel updatedMake)
    {
        var makeDto = new MakeDto()
        {
            Id = updatedMake.Id,
            Name = updatedMake.Name,
            Description = updatedMake.Description,
            Status = Convert.ToByte(updatedMake.Status ? 1 : 0)
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(makeDto), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(ApiBase, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnMake = JsonConvert.DeserializeObject<MakeListViewModel>(data);
        
        return returnMake ?? new MakeListViewModel();
    }
    
    public async Task<MakeListViewModel> CreateMakeAsync(MakeListViewModel makeToAdd)
    {
        var makeDto = new MakeDto()
        {
            Id = makeToAdd.Id,
            Name = makeToAdd.Name,
            Description = makeToAdd.Description,
            Status = Convert.ToByte(1)
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(makeDto), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(ApiBase, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnMake = JsonConvert.DeserializeObject<MakeListViewModel>(data);
        
        return returnMake ?? new MakeListViewModel();
    }

    public async Task DeleteMakeAsync(int id)
    {
        var response = await _httpClient.DeleteAsync(ApiBase + $"/{id}");
    }
}
