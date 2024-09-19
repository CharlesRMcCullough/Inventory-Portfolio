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

    private readonly MakeListViewModel _model = new();
    private string _makePrompt = string.Empty;
    private bool _isLoading = false;

    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var make = await Integration.GetMakeByIdAsync(Id);
            _model.Id = make.Id;
            _model.Name = make.Name;
            _model.Description = make.Description;
            _model.Status = make.Status;
        }
        
        _makePrompt = IsAdd ? "Make Add" : $"Make: {_model.Name}";
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
            await Integration.UpdateMakeAsync(_model);
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
            _model.Id = 0;
            await Integration.CreateMakeAsync(_model);
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
}