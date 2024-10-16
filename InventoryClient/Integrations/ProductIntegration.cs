using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace InventoryClient.Integrations;

public class ProductIntegration : IProductIntegration
{

    private readonly HttpClient _httpClient;

    public ProductIntegration(IOptions<ApiSettings> apiSettings)
    {
        var settings = apiSettings.Value;
        _httpClient = new HttpClient { BaseAddress = new Uri(settings.ApiUrl + settings.ProductApiBase) };
    }
    public async Task<IEnumerable<ProductListViewModel>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductListViewModel>>(data) ?? new List<ProductListViewModel>();
        }

        return new List<ProductListViewModel>();
    }

    public async Task<ProductListViewModel> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");

        if (response.IsSuccessStatusCode)
        {
            var productData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductListViewModel>(productData) ??
                   new ProductListViewModel();
        }

        return new ProductListViewModel();
    }
    
    public async Task<IEnumerable<ProductListViewModel>> GetProductsByCategoryIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/byCategory/{id}");
        var returnProducts = new List<ProductListViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnProducts = JsonConvert.DeserializeObject<List<ProductListViewModel>>(data);
        }

        if (returnProducts == null)
            return new List<ProductListViewModel>();

        return returnProducts;
    }


    public async Task<IEnumerable<DropdownViewModel>> GetProductsForDropdownsAsync()
    {
        try
        {
            var httpResponse = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/dropdowns");
            httpResponse.EnsureSuccessStatusCode();

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<DropdownViewModel>>(responseContent) ?? new List<DropdownViewModel>();

            return products;
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, $"Error retrieving products from the database: {ex.Message}");
            return new List<DropdownViewModel>();
        }
        catch (JsonException ex)
        {
            Log.Error(ex, $"Error deserializing products from the database: {ex.Message}");
            return new List<DropdownViewModel>();
        }
    }

    public async Task<ProductListViewModel> UpdateProductAsync(ProductListViewModel productToUpdate)
    {
        var productDto = new ProductDto()

        {
            Id = productToUpdate.Id,
            Name = productToUpdate.Name,
            Description = productToUpdate.Description,
            CategoryId = productToUpdate.CategoryId,
            MakeId = productToUpdate.MakeId,
            Price = productToUpdate.Price,
            Quantity = 0,
            ModelId = productToUpdate.ModelId,
            Notes = productToUpdate.Notes,
            Status = productToUpdate.Status,
        };

        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(_httpClient.BaseAddress, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var updatedProduct = JsonConvert.DeserializeObject<ProductListViewModel>(data);

        return updatedProduct ?? new ProductListViewModel();
    }

    public async Task<ProductListViewModel> CreateProductAsync(ProductListViewModel productToAdd)
    {
        var productDto = new ProductDto
        {
            Name = productToAdd.Name,
            Description = productToAdd.Description,
            CategoryId = productToAdd.CategoryId,
            MakeId = productToAdd.MakeId,
            Price = productToAdd.Price,
            Quantity = 0,
            ModelId = productToAdd.ModelId,
            Notes = productToAdd.Notes,
            Status = true,
        };

        using var jsonContent = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, jsonContent);

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsStringAsync();
        var returnProduct = JsonConvert.DeserializeObject<ProductListViewModel>(data);

        return returnProduct ?? new ProductListViewModel();
    }

    public async Task DeleteProductAsync(int id)
    {
        await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}");
    }
}