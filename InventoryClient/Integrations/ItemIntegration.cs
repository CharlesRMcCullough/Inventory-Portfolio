using System.Text;
using API.DTOs;
using InventoryClient.Integrations.Interfaces;
using InventoryClient.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InventoryClient.Integrations;

public class ItemIntegration : IItemIntegration
{
    private readonly HttpClient _httpClient;

    public ItemIntegration(IOptions<ApiSettings> apiSettings)
    {
        var settings = apiSettings.Value;
        _httpClient = new HttpClient { BaseAddress = new Uri(settings.ApiUrl + settings.ItemApiBase) };
    }

    public async Task<IReadOnlyList<ItemListViewModel>> GetItemsAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);

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
        
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{productId}");

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
      //  var response = await HttpClient.GetAsync($"{ApiBase}/item/{itemId}");
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/item/{itemId}");

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ItemListViewModel>(responseData) ?? new ItemListViewModel();
        }

        return new ItemListViewModel();
    }

    public async Task<ItemListViewModel> CreateItemAsync(ItemListViewModel itemToAdd)
    {
        var itemDto = new ItemDto
        {
            SerialNumber = itemToAdd.SerialNumber,
            TagId = itemToAdd.TagId,
            ProductId = itemToAdd.ProductId,
            Price = itemToAdd.Price,
            Notes = itemToAdd.Notes,
            History = string.Empty,
            Location = string.Empty,
            Status = itemToAdd.Status,
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, jsonContent);

        var data = await response.Content.ReadAsStringAsync();
        var returnItem = JsonConvert.DeserializeObject<ItemListViewModel>(data);

        return returnItem ?? new ItemListViewModel();
    }

    public async Task<ItemListViewModel> UpdateItemAsync(ItemListViewModel itemToUpdate)
    {
        var itemDto = new ItemDto
        {
            Id = itemToUpdate.Id,
            ProductId = itemToUpdate.ProductId,
            SerialNumber = itemToUpdate.SerialNumber,
            TagId = itemToUpdate.TagId,
            CheckInDate = itemToUpdate.CheckInDate,
            CheckOutDate = itemToUpdate.CheckOutDate,
            Price = itemToUpdate.Price,
            Notes = itemToUpdate.Notes,
            History = itemToUpdate.History,
            Location = itemToUpdate.Location,
            Status = itemToUpdate.Status,
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(_httpClient.BaseAddress, jsonContent);

        var data = await response.Content.ReadAsStringAsync();
        var updatedItem = JsonConvert.DeserializeObject<ItemListViewModel>(data);

        return updatedItem ?? new ItemListViewModel();
    }

    public async Task DeleteItemAsync(int id)
    {
        await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}");
    }
}