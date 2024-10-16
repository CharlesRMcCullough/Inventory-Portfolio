using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class MakeIntegration : IMakeIntegration
{
    private readonly HttpClient _httpClient;

    public MakeIntegration(IOptions<ApiSettings> apiSettings)
    {
        var settings = apiSettings.Value;
        _httpClient = new HttpClient { BaseAddress = new Uri(settings.ApiUrl + settings.MakeApiBase) };
    }

    public async Task<IEnumerable<MakeListViewModel>> GetMakesAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
        var returnMakes = new List<MakeListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnMakes = JsonConvert.DeserializeObject<List<MakeListViewModel>>(data);
        }

        if (returnMakes == null)
            return new List<MakeListViewModel>();

        return returnMakes;
    }

    public async Task<IEnumerable<MakeListViewModel>> GetMakesByCategoryIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/byCategory/{id}");
        var returnMake = new List<MakeListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnMake = JsonConvert.DeserializeObject<List<MakeListViewModel>>(data);
        }

        if (returnMake == null)
            return new List<MakeListViewModel>();

        return returnMake;
    }

    public async Task<MakeListViewModel> GetMakeByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");
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
    
    public async Task<IEnumerable<DropdownViewModel>> GetMakesForDropdownsAsync(int categoryId = 0)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/dropdowns/{categoryId}");
        var returnMakes = new List<DropdownViewModel>(categoryId);
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnMakes = JsonConvert.DeserializeObject<List<DropdownViewModel>>(data);
        }

        if (returnMakes == null)
            return new List<DropdownViewModel>();

        return returnMakes;
    }

    public async Task<MakeListViewModel> UpdateMakeAsync(MakeListViewModel updatedMake)
    {
        var makeDto = new MakeDto()
        {
            Id = updatedMake.Id,
            Name = updatedMake.Name,
            Description = updatedMake.Description,
            Status = updatedMake.Status,
            CategoryId = updatedMake.CategoryId
        };

        using StringContent jsonContent =
            new StringContent(JsonConvert.SerializeObject(makeDto), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(_httpClient.BaseAddress, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var returnMake = JsonConvert.DeserializeObject<MakeListViewModel>(data);

        return returnMake ?? new MakeListViewModel();
    }

    public async Task<MakeListViewModel> CreateMakeAsync(MakeListViewModel makeToAdd)
    {
        var makeDto = new MakeDto()
        {
            Id = makeToAdd.Id,
            CategoryId = makeToAdd.CategoryId,
            Name = makeToAdd.Name,
            Description = makeToAdd.Description,
            Status = makeToAdd.Status
        };

        using StringContent jsonContent =
            new StringContent(JsonConvert.SerializeObject(makeDto), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var returnMake = JsonConvert.DeserializeObject<MakeListViewModel>(data);

        return returnMake ?? new MakeListViewModel();
    }

    public async Task DeleteMakeAsync(int id)
    {
        await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}");
    }
}