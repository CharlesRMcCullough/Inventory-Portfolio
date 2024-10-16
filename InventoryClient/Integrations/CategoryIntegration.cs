using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class CategoryIntegration : ICategoryIntegration
{
    
    private readonly HttpClient _httpClient;

    public CategoryIntegration(IOptions<ApiSettings> apiSettings)
    {
        var settings = apiSettings.Value;
        _httpClient = new HttpClient { BaseAddress = new Uri(settings.ApiUrl + settings.CategoryApiBase) };
    }
   
    public async Task<IEnumerable<CategoryListViewModel>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
        var returnCategories = new List<CategoryListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnCategories = JsonConvert.DeserializeObject<List<CategoryListViewModel>>(data);
        }

        if (returnCategories == null)
            return new List<CategoryListViewModel>();

        return returnCategories;
    }
    
    public async Task<IEnumerable<DropdownViewModel>> GetCategoriesForDropdownsAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress+ "/dropdowns");
        var returnCategories = new List<DropdownViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnCategories = JsonConvert.DeserializeObject<List<DropdownViewModel>>(data);
        }

        if (returnCategories == null)
            return new List<DropdownViewModel>();

        return returnCategories;
    }

    public async Task<CategoryListViewModel> GetCategoryByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");
        var returnCategory = new CategoryListViewModel();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnCategory = JsonConvert.DeserializeObject<CategoryListViewModel>(data);
        }

        if (returnCategory == null)
            return new CategoryListViewModel();

        return returnCategory;
    }

    public async Task<CategoryListViewModel> UpdateCategoryAsync(CategoryListViewModel updatedCategory)
    {
        var categoryDto = new CategoryDto()
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            Description = updatedCategory.Description,
            Status = updatedCategory.Status
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(_httpClient.BaseAddress, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnCategory = JsonConvert.DeserializeObject<CategoryListViewModel>(data);
        
        return returnCategory ?? new CategoryListViewModel();
    }
    
    public async Task<CategoryListViewModel> CreateCategoryAsync(CategoryListViewModel categoryToAdd)
    {
        var categoryDto = new CategoryDto()
        {
            Id = categoryToAdd.Id,
            Name = categoryToAdd.Name,
            Description = categoryToAdd.Description,
            Status = categoryToAdd.Status
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnCategory = JsonConvert.DeserializeObject<CategoryListViewModel>(data);
        
        return returnCategory ?? new CategoryListViewModel();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}");
    }
}