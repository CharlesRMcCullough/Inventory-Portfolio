using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace InventoryClient.Components.Pages.Categories;

public partial class CategoryEdit : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Parameter] public int Mode { get; set; }

    private bool IsView => Mode == 0;
    private bool IsAdd => Mode == 2;

    private CategoryListViewModel _model = new();
    private string _categoryPrompt = string.Empty;
    private bool _isLoading = false;

    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var category = await Integration.GetCategoryByIdAsync(Id);
            _model.Id = category.Id;
            _model.Name = category.Name;
            _model.Description = category.Description;
            _model.Status = category.Status;
        }
        
        _categoryPrompt = IsAdd ? "Category Add" : $"Category: {_model.Name}";
    }

    private async Task OnValidSubmit(EditContext context)
    {
        if (IsAdd)
        {
            await CreateCategory();
        }
        else
        {
            await UpdateCategory();
        }

        Navigation.NavigateTo("/Categories");
    }
    
    private async Task UpdateCategory()
    {
        try
        {
            _isLoading = true;
            await Integration.UpdateCategoryAsync(_model);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error updating category! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task CreateCategory()
    {
        try
        {
            _isLoading = true;
            _model.Id = 0;
            await Integration.CreateCategoryAsync(_model);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error creating category! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    private void OnCancel()
    {
        Navigation.NavigateTo("/Categories");
    }
}