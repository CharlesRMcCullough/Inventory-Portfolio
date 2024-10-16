using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace InventoryClient.Components.Pages.Makes;

public partial class MakeEdit : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Parameter] public int Mode { get; set; }

    private bool IsView => Mode == 0;
    private bool IsAdd => Mode == 2;

    private MakeListViewModel ViewModel { get; set; } = new MakeListViewModel();
    private string _makePrompt = string.Empty;
    private bool _isLoading = false;

    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var make = await Integration.GetMakeByIdAsync(Id);
            ViewModel.Id = make.Id;
            ViewModel.Name = make.Name;
            ViewModel.Description = make.Description;
            ViewModel.Status = make.Status;
            ViewModel.CategoryId = make.CategoryId;
        }
        
        _makePrompt = IsAdd ? "Make Add" : $"Make: {ViewModel.Name}";
    }

    private async Task OnValidSubmit(EditContext context)
    {
        if (IsAdd)
        {
            await CreateMake();
        }
        else
        {
            await UpdateMake();
        }

        Navigation.NavigateTo("/Makes");
    }
    
    private async Task UpdateMake()
    {
        try
        {
            _isLoading = true;
            await Integration.UpdateMakeAsync(ViewModel);
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

    private async Task CreateMake()
    {
        try
        {
            _isLoading = true;
            ViewModel.Id = 0;
            ViewModel.Status = true;
            await Integration.CreateMakeAsync(ViewModel);
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
        Navigation.NavigateTo("/Makes");
    }
    
    private async Task OnCategoryChange(int categoryId)
    {
        ViewModel.CategoryId = categoryId;
        await Task.CompletedTask;
    }
}