using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InventoryClient.Components.Pages.Products;

public partial class ProductList : ComponentBase
{
    private IEnumerable<ProductListViewModel> Products { get; set; }
    private bool _isLoading;
    
    protected override async Task OnInitializedAsync()
    {
        await GetProductsAsync();
    }
    
    private async Task GetProductsAsync()
    {
        try
        {
            _isLoading = true;
            Products = await Integration.GetProductsAsync();
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load products! {e.Message}", Severity.Error);
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
            { x => x.ContentText, "Do you really want to delete this product?" },
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
            //    await Integration.DeleteCategoryAsync(id);
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
            
        await GetProductsAsync();
    }

    private void OnEdit(int id)
    {
       // Navigation.NavigateTo($"/CategoryEdit/{id}/1");
    }
    private void OnView(int id)
    {
     //   Navigation.NavigateTo($"/CategoryEdit/{id}/0");
    }

    private void OnAdd()
    {
       // Navigation.NavigateTo("/CategoryEdit/0/2");
    }
}