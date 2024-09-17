using System.Text;
using API.DTOs;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public interface ICategoryIntegration
{
    Task<IEnumerable<CategoryListViewModel>> GetCategoriesAsync();
    Task<CategoryListViewModel> GetCategoryByIdAsync(int id);
    Task<CategoryListViewModel> CreateCategoryAsync(CategoryListViewModel categoryToAdd);
    Task<CategoryListViewModel> UpdateCategoryAsync(CategoryListViewModel updatedCategory);
    Task DeleteCategoryAsync(int id);
}

public class CategoryIntegration : ICategoryIntegration
{
    private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:7001")
    };
    
    public async Task<IEnumerable<CategoryListViewModel>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("/api/categories");
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
        var response = await _httpClient.GetAsync($"/api/categories/{id}");
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
        
        var response = await _httpClient.PutAsync($"/api/categories", jsonContent);
        
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
        
        var response = await _httpClient.PostAsync($"/api/categories", jsonContent);
        
        var data = response.Content.ReadAsStringAsync().Result;
        var returnCategory = JsonConvert.DeserializeObject<CategoryListViewModel>(data);
        
        return returnCategory ?? new CategoryListViewModel();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/categories/{id}");
    }
}