using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InventoryClient.Components.Pages.Items;

public partial class ItemList : ComponentBase
{
    private IEnumerable<ItemListViewModel>? Items { get; set; }
    private bool _isLoading;
    private int _selectedProduct;
    
    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;
        //await LoadItemsAsync(_selectedProduct);
    }
    
    private async Task LoadItemsAsync(int productId)
    {
        try
        {
            _isLoading = true;
            Items = await Integration.GetItemsByProductId(productId);
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load items! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    private async Task OnDeleteAsync(int id)
    {
        var parameters = new DialogParameters<DeleteDialog>
        {
            { x => x.ContentText, "Do you really want to delete this item?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialogResult = await DialogService.ShowAsync<DeleteDialog>("Delete", parameters, options);
        var result = await dialogResult.Result;

        if (result is { Canceled: false })
        {
            try
            {
                _isLoading = true;
        //        await Integration.DeleteProductAsync(id);
                Snackbar.Add("Delete successful!", Severity.Success);
            }
            catch (Exception e)
            {
                Snackbar.Add($"Error deleting product!: {e.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
            
        }
            
        await LoadItemsAsync(1);
    }
    
    private async Task OnProductChange(int productId)
    {
        try
        {
            _isLoading = true;
            Items = await Integration.GetItemsByProductId(productId);
            _selectedProduct = productId;
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load items! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void OnEdit(int id)
    {
        //Navigation.NavigateTo($"/ProducEdit/{id}/1");
    }
    private void OnView(int id)
    {
      //  Navigation.NavigateTo($"/ProductEdit/{id}/0");
    }

    private void OnAdd()
    {
        //Navigation.NavigateTo("/ProductEdit/0/2");
    }
}