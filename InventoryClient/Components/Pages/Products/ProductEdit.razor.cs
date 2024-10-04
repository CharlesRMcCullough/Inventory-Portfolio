using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace InventoryClient.Components.Pages.Products;

public partial class ProductEdit : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Parameter] public int Mode { get; set; }

    private bool IsView => Mode == 0;
    private bool IsAdd => Mode == 2;

    private ProductListViewModel ViewModel { get; set; } = new ProductListViewModel();
    private string _productPrompt = string.Empty;
    private bool _isLoading = false;

        protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var product = await Integration.GetProductByIdAsync(Id);
            ViewModel.Id = product.Id;
            ViewModel.Name = product.Name;
            ViewModel.Description = product.Description;
            ViewModel.Status = product.Status;
            ViewModel.CategoryId = product.CategoryId;
            ViewModel.Quantity = product.Quantity;
            ViewModel.Price = product.Price;
        }
        
        _productPrompt = IsAdd ? "Product Add" : $"Product: {ViewModel.Name}";
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

        Navigation.NavigateTo("/Products");
    }
    
    private async Task UpdateProduct()
    {
        try
        {
            _isLoading = true;
            await Integration.UpdateProductAsync(ViewModel);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error updating product! {e.Message}", Severity.Error);
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
            await Integration.CreateProductAsync(ViewModel);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error creating product! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    private void OnCancel()
    {
        Navigation.NavigateTo("/Products");
    }
    
    private async Task OnCategoryChange(int categoryId)
    {
        ViewModel.CategoryId = categoryId;
        await Task.CompletedTask;
    }
}