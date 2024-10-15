using System.Text;
using API.DTOs;
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
            return JsonConvert.DeserializeObject<IReadOnlyList<ItemListViewModel>>(responseData) ??
                   Array.Empty<ItemListViewModel>();
        }

        return Array.Empty<ItemListViewModel>();
    }

    public async Task<IReadOnlyList<ItemListViewModel>> GetItemsByProductId(int productId)
    {
        var requestUri = $"{ApiBase}/{productId}";
        var response = await HttpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            return Array.Empty<ItemListViewModel>();
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var items = DeserializeItems(responseContent);
        SetExpectedReturnDate(items);

        return items;
    }

    private IReadOnlyList<ItemListViewModel> DeserializeItems(string responseContent)
    {
        var settings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            NullValueHandling = NullValueHandling.Ignore
        };

        var items = JsonConvert.DeserializeObject<IReadOnlyList<ItemListViewModel>>(responseContent, settings);
        return items ?? Array.Empty<ItemListViewModel>();
    }

    private void SetExpectedReturnDate(IReadOnlyList<ItemListViewModel> items)
    {
        foreach (var item in items)
        {
            item.IsCheckedOut = item.CheckOutDate != null;
            if (item.CheckInDate != null)
            {
                item.ExpectedReturnDate = item.CheckInDate?.ToString("MM/dd/yyyy");
            }
        }
    }

    public async Task<ItemListViewModel> GetItemById(int itemId)
    {
        var response = await HttpClient.GetAsync($"{ApiBase}/item/{itemId}");

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ItemListViewModel>(responseData) ?? new ItemListViewModel();
        }

        return new ItemListViewModel();
    }

    public async Task<ItemListViewModel> CreateItemAsync(ItemListViewModel itemToAdd)
    {
        var itemDto = new ItemDto()
        {
            Id = itemToAdd.Id,
            SerialNumber = itemToAdd.SerialNumber,
            TagId = itemToAdd.TagId,
            ProductId = itemToAdd.ProductId,
            Price = itemToAdd.Price,
            Notes = itemToAdd.Notes,
            Status = itemToAdd.Status,
        };

        using StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8,
            "application/json");

        var response = await HttpClient.PostAsync(ApiBase, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var returnProduct = JsonConvert.DeserializeObject<ItemListViewModel>(data);


        return new ItemListViewModel();
    }

    public async Task<ItemListViewModel> UpdateItemAsync(ItemListViewModel itemToUpdate)
    {
        var itemDto = new ItemDto()
        {
            Id = itemToUpdate.Id,
            ProductId = itemToUpdate.ProductId,
            SerialNumber = itemToUpdate.SerialNumber,
            TagId = itemToUpdate.TagId,
            CheckInDate = itemToUpdate.CheckInDate,
            CheckOutDate = itemToUpdate.CheckOutDate,
            Price = itemToUpdate.Price,
            Notes = itemToUpdate.Notes,
            Status = itemToUpdate.Status,
        };

        using StringContent jsonContent =
            new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8, "application/json");

        var response = await HttpClient.PutAsync(ApiBase, jsonContent);

        var data = response.Content.ReadAsStringAsync().Result;
        var updatedItem = JsonConvert.DeserializeObject<ItemListViewModel>(data);

        return updatedItem ?? new ItemListViewModel();
    }

    public async Task DeleteItemAsync(int id)
    {
        await HttpClient.DeleteAsync($"{ApiBase}/{id}");
    }
}