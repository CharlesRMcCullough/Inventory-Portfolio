using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class MakeIntegration : IMakeIntegration
{
    private const string ApiBase = "/api/makes";
    private const string ApiUrl = "http://localhost:7001";

    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri(ApiUrl)
    };

    public async Task<IEnumerable<MakeListViewModel>> GetMakesAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase);
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
        var response = await HttpClient.GetAsync(ApiBase + $"/byCategory/{id}");
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
        var response = await HttpClient.GetAsync(ApiBase + $"/{id}");
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
    
    public async Task<IEnumerable<DropdownViewModel>> GetMakesForDropdownsAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase + "/dropdowns");
        var returnMakes = new List<DropdownViewModel>();
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
            Status = Convert.ToByte(updatedMake.Status ? 1 : 0),
            CategoryId = updatedMake.CategoryId
        };

        using StringContent jsonContent =
            new StringContent(JsonConvert.SerializeObject(makeDto), Encoding.UTF8, "application/json");

        var response = await HttpClient.PutAsync(ApiBase, jsonContent);

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

        using StringContent jsonContent =
            new StringContent(JsonConvert.SerializeObject(makeDto), Encoding.UTF8, "application/json");

        var response = await HttpClient.PostAsync(ApiBase, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var returnMake = JsonConvert.DeserializeObject<MakeListViewModel>(data);

        return returnMake ?? new MakeListViewModel();
    }

    public async Task DeleteMakeAsync(int id)
    {
        await HttpClient.DeleteAsync(ApiBase + $"/{id}");
    }
}