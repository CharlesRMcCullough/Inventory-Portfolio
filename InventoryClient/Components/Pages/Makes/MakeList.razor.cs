using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InventoryClient.Components.Pages.Makes;

public partial class MakeList : ComponentBase
{
    private IEnumerable<MakeListViewModel>? Makes  { get; set; }
    private bool _isLoading;
    private int selectedCategory = 0;
    
    protected override async Task OnInitializedAsync()
    {
        await GetMakesAsync();
    }
    
    private void OnEdit(int id)
    {
        Navigation.NavigateTo($"/MakeEdit/{id}/1");
    }

    private async Task OnDeleteAsync(int id)
    {
        var parameters = new DialogParameters<DeleteDialog>
        {
            { x => x.ContentText, "Do you really want to delete this Make?" },
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
                await Integration.DeleteMakeAsync(id);
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
            
        await GetMakesAsync();

    }

    private void OnView(int id)
    {
        Navigation.NavigateTo($"/MakeEdit/{id}/0");
    }

    private void OnAdd()
    {
        Navigation.NavigateTo("/MakeEdit/0/2");
    }

    private async Task GetMakesAsync()
    {
        try
        {
            _isLoading = true;
            Makes = await Integration.GetMakesAsync();
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load Makes! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task OnCategoryChange(int categoryId)
    {
        try
        {
            _isLoading = true;
            Makes = await Integration.GetMakesByCategoryIdAsync(categoryId);
            selectedCategory = categoryId;
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load Makes! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
}