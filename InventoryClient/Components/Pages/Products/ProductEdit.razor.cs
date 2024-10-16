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
    private bool _isLoading;
    public bool CategoryDisabled { get; set; }
    public bool MakeDisabled { get; set; }
    public bool ModelDisabled { get; set; }

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
            ViewModel.MakeId = product.MakeId;
            ViewModel.ModelId = product.ModelId;
            ViewModel.Quantity = product.Quantity;
            ViewModel.AvailableQuantity = product.AvailableQuantity;
            ViewModel.Price = product.Price;
            ViewModel.Notes = product.Notes;

            CategoryDisabled = false;
            ModelDisabled = false;
            MakeDisabled = false;
        }
        else
        {
            CategoryDisabled = false;
            MakeDisabled = true;
            ModelDisabled = true;
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
        MakeDisabled = false;
        ModelDisabled = true;
        ViewModel.MakeId = 0;
        await Task.CompletedTask;
    }
    private async Task OnMakeChange(int makeId)
    {
        ViewModel.MakeId = makeId;
        ModelDisabled = false;
        ViewModel.ModelId = 0;
        await Task.CompletedTask;
    }
    private async Task OnModelChange(int modelId)
    {
        ViewModel.ModelId = modelId;
        await Task.CompletedTask;
    }
}