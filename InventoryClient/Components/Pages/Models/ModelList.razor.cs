using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InventoryClient.Components.Pages.Models;

public partial class ModelList : ComponentBase
{
    private IEnumerable<ModelListViewModel>? Models  { get; set; }
    private bool _isLoading;
    
    protected override async Task OnInitializedAsync()
    {
        await GetModelsAsync();
    }
    
    private void OnEdit(int id)
    {
        Navigation.NavigateTo($"/ModelEdit/{id}/1");
    }

    private async Task OnDeleteAsync(int id)
    {
        var parameters = new DialogParameters<DeleteDialog>
        {
            { x => x.ContentText, "Do you really want to delete this Model?" },
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
                await Integration.DeleteModelAsync(id);
                Snackbar.Add("Delete successful!", Severity.Success);
            }
            catch (Exception e)
            {
                Snackbar.Add($"Error deleting make: {e.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
            
        }
            
        await GetModelsAsync();

    }

    private void OnView(int id)
    {
        Navigation.NavigateTo($"/ModelEdit/{id}/0");
    }

    private void OnAdd()
    {
        Navigation.NavigateTo("/ModelEdit/0/2");
    }

    private async Task GetModelsAsync()
    {
        try
        {
            _isLoading = true;
            Models = await Integration.GetModelsAsync();
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load Models! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
}