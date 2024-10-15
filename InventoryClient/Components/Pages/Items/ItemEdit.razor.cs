using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace InventoryClient.Components.Pages.Items;

public partial class ItemEdit : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Parameter] public int Mode { get; set; }

    private bool IsView => Mode == 0;
    private bool IsAdd => Mode == 2;
    private bool IsEdit => Mode == 1;

    private ItemListViewModel ViewModel { get; set; } = new ItemListViewModel();
    private string _productPrompt = string.Empty;
    private bool _isLoading;

    
    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var item = await Integration.GetItemById(Id);
            ViewModel.Id = item.Id;
            ViewModel.Name = item.Name;
            ViewModel.SerialNumber = item.SerialNumber;
            ViewModel.TagId = item.TagId;
            ViewModel.Price = item.Price;
            ViewModel.Notes = item.Notes;
            ViewModel.Description = item.Description;
            ViewModel.CheckOutDate = item.CheckOutDate;
            ViewModel.CheckInDate = item.CheckInDate;
            ViewModel.Status = item.Status;
            ViewModel.ProductId = item.ProductId;
            ViewModel.IsCheckedOut = item.CheckOutDate != null;
        }
        
        _productPrompt = IsAdd ? "Item Add" : $"Item: {ViewModel.Name}";
    }
    private async Task OnValidSubmit(EditContext context)
    {
        if (IsAdd)
        {
            await CreateProduct();
        }
        else
        {
            await UpdateProduct();
        }

        Navigation.NavigateTo("/Items");
    }
    
    private async Task UpdateProduct()
    {
        try
        {
            _isLoading = true;
            await Integration.UpdateItemAsync(ViewModel);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error updating item! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task CreateProduct()
    {
        try
        {
            _isLoading = true;
            ViewModel.Id = 0;
            ViewModel.ProductId = Id;
            ViewModel.Status = true;
            await Integration.CreateItemAsync(ViewModel);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error creating item! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    private void OnCancel()
    {
        Navigation.NavigateTo("/Items");
    }

    private async Task OnCheckIn()
    {
        ViewModel.CheckOutDate = null;
        ViewModel.CheckInDate = null;
        ViewModel.Notes = $"{ViewModel.Notes} Checked in on {DateTime.Now:yyyy-MM-dd}\n";
        await UpdateProduct();
        Navigation.NavigateTo("/Items");
    }
    private async Task OnCheckOut()
    {
        if (ViewModel.CheckOutDate != null && ViewModel.CheckInDate != null)
        {
            ViewModel.Notes = $"{ViewModel.Notes} Checkout on {DateTime.Now:yyyy-MM-dd HH}\n";
            await UpdateProduct();
            Navigation.NavigateTo("/Items");
        }
        else
        {
            Snackbar.Add($"The Checkout date and Expected Cheek In date are required", Severity.Error);
        }
    }
}