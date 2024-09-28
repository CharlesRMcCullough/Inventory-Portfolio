using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class ProductIntegration : IProductIntegration
{
    private const string ApiBase = "/api/categories";
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
    
    public async Task<IEnumerable<DropdownViewModel>> GetProductsForDropdownsAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase + "/dropdowns");
        var returnProducts = new List<DropdownViewModel>();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnProducts = JsonConvert.DeserializeObject<List<DropdownViewModel>>(data);
        }

        if (returnProducts == null)
            return new List<DropdownViewModel>();

        return returnProducts;
    }
    
    public async Task<ProductListViewModel> GetProductByIdAsync(int id)
    {
        var response = await HttpClient.GetAsync(ApiBase + $"/{id}");
        var returnProduct = new ProductListViewModel();
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            returnProduct = JsonConvert.DeserializeObject<ProductListViewModel>(data);
        }

        if (returnProduct == null)
            return new ProductListViewModel();

        return returnProduct;
    }
    
    
}