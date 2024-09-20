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

    private readonly CategoryListViewModel _model = new();
    private string _makePrompt = string.Empty;
    private bool _isLoading;

    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var make = await Integration.GetCategoryByIdAsync(Id);
            _model.Id = make.Id;
            _model.Name = make.Name;
            _model.Description = make.Description;
            _model.Status = make.Status;
        }
        
        _makePrompt = IsAdd ? "Category Add" : $"Category: {_model.Name}";
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
            Snackbar.Add($"Error updating make! {e.Message}", Severity.Error);
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
            Snackbar.Add($"Error creating make! {e.Message}", Severity.Error);
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