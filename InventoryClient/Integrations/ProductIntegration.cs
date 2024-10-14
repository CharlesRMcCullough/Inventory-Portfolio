using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class ProductIntegration : IProductIntegration
{
    private const string ApiBase = "/api/products";
    private const string ApiUrl = "http://localhost:7001";

    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri(ApiUrl)
    };

    public async Task<IEnumerable<ProductListViewModel>> GetProductsAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase);
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

    public async Task<ProductListViewModel> GetProductByIdAsync(int id)
    {
        var response = await HttpClient.GetAsync($"{ApiBase}/{id}");

        if (response.IsSuccessStatusCode)
        {
            var productData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductListViewModel>(productData) ??
                   new ProductListViewModel();
        }

        return new ProductListViewModel();
    }


    public async Task<IEnumerable<DropdownViewModel>> GetProductsForDropdownsAsync()
    {
        try
        {
            var response = await HttpClient.GetAsync($"{ApiBase}/dropdowns");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var returnProducts = JsonConvert.DeserializeObject<List<DropdownViewModel>>(result) ?? new List<DropdownViewModel>();

            return returnProducts;
        }
        catch (HttpRequestException)
        {
            return new List<DropdownViewModel>(); // Return an empty list on error
        }
        catch (JsonException)
        {
            // Log the exception (logging mechanism not shown here)
            return new List<DropdownViewModel>(); // Return an empty list on deserialization error
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

        var response = await HttpClient.PutAsync(ApiBase, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var updatedProduct = JsonConvert.DeserializeObject<ProductListViewModel>(data);

        return updatedProduct ?? new ProductListViewModel();
    }

    public async Task<ProductListViewModel> CreateProductAsync(ProductListViewModel productToAdd)
    {
        var productDto = new ProductDto()
        {
            Name = productToAdd.Name,
            Description = productToAdd.Description,
            CategoryId = productToAdd.CategoryId,
            MakeId = productToAdd.MakeId,
            Price = productToAdd.Price,
            Quantity = 0,
            ModelId = productToAdd.ModelId,
            Notes = productToAdd.Notes,
            Status = true
        };

        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8,
            "application/json");

        var response = await HttpClient.PostAsync(ApiBase, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var returnProduct = JsonConvert.DeserializeObject<ProductListViewModel>(data);


        return new ProductListViewModel();
    }

    public async Task DeleteProductAsync(int id)
    {
        await HttpClient.DeleteAsync($"{ApiBase}/{id}");
    }
}