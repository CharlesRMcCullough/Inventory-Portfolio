using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryClient.Integrations;

public class ItemIntegration : IItemIntegration
{
    private const string ApiBase = "/api/items";
    private const string ApiUrl = "http://localhost:7001";
    
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri(ApiUrl)
    };
    
    public async Task<IReadOnlyList<ItemListViewModel>> GetItemsAsync()
    {
        var response = await HttpClient.GetAsync(ApiBase);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IReadOnlyList<ItemListViewModel>>(responseData) ?? Array.Empty<ItemListViewModel>();
        }

        return Array.Empty<ItemListViewModel>();
    }
    
    public async Task<IReadOnlyList<ItemListViewModel>> GetItemsByProductId(int productId)
    {
        var response = await HttpClient.GetAsync($"{ApiBase}/{productId}");

        try
        {
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<IReadOnlyList<ItemListViewModel>>(responseData) ?? Array.Empty<ItemListViewModel>();

            foreach (var item in items)
            {
                item.IsCheckedOut = item.CheckOutDate != null;
            }

            return items;
        }
        catch (JsonException)
        {
            return Array.Empty<ItemListViewModel>();
        }
    }
    
    public async Task<ItemListViewModel> GetItemByItemId(int itemId)
    {
        var response = await HttpClient.GetAsync($"{ApiBase}/item/{itemId}");

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ItemListViewModel>(responseData) ?? new ItemListViewModel();
        }

        return new ItemListViewModel();
    }
}