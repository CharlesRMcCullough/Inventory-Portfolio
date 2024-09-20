using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace InventoryClient.Components.Pages.Models;

public partial class ModelEdit : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Parameter] public int Mode { get; set; }

    private bool IsView => Mode == 0;
    private bool IsAdd => Mode == 2;

    private ModelListViewModel ViewModel { get; set; } = new ModelListViewModel();
    private string _modelPrompt = string.Empty;
    private bool _isLoading;

    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var make = await Integration.GetModelByIdAsync(Id);
            ViewModel.Id = make.Id;
            ViewModel.Name = make.Name;
            ViewModel.Description = make.Description;
            ViewModel.Status = make.Status;
        }
        
        _modelPrompt = IsAdd ? "Model Add" : $"Model: {ViewModel.Name}";
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

        Navigation.NavigateTo("/Models");
    }
    
    private async Task UpdateMake()
    {
        try
        {
            _isLoading = true;
            await Integration.UpdateModelAsync(ViewModel);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error updating Model! {e.Message}", Severity.Error);
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
            await Integration.CreateModelAsync(ViewModel);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Error creating Model! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    private void OnCancel()
    {
        Navigation.NavigateTo("/Models");
    }
}