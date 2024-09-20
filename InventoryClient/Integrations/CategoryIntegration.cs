using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class CategoryIntegration : ICategoryIntegration
{
    
    private const string ApiBase = "/api/categories";
    private const string ApiUrl = "http://localhost:7001";
    
    
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri(ApiUrl)
    };
   
    public async Task<IEnumerable<CategoryListViewModel>> GetCategoriesAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase);
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

    public async Task<CategoryListViewModel> GetCategoryByIdAsync(int id)
    {
        var response = await HttpClient.GetAsync(ApiBase + $"/{id}");
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
            Status = Convert.ToByte(updatedCategory.Status ? 1 : 0)
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        
        var response = await HttpClient.PutAsync(ApiBase, jsonContent);
        
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
            Status = Convert.ToByte(1)
        };
        
        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");
        
        var response = await HttpClient.PostAsync(ApiBase, jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnCategory = JsonConvert.DeserializeObject<CategoryListViewModel>(data);
        
        return returnCategory ?? new CategoryListViewModel();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await HttpClient.DeleteAsync(ApiBase + $"/{id}");
    }
}