using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InventoryClient.Components.Pages.Categories;

public partial class CategoryList : ComponentBase
{
    private IEnumerable<CategoryListViewModel>? Categories { get; set; }
    private bool _isLoading;
    private string _searchText = string.Empty;
    private int _selectedPage;
    private MudDataGrid<CategoryListViewModel> _grid;
    
    protected override async Task OnInitializedAsync()
    {
        await GetCategoriesAsync();
    }
    
    private void OnEdit(int id)
    {
        Navigation.NavigateTo($"/CategoryEdit/{id}/1");
    }

    private async Task OnDeleteAsync(int id)
    {
        var parameters = new DialogParameters<DeleteDialog>
        {
            { x => x.ContentText, "Do you really want to delete this category?" },
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
                await Integration.DeleteCategoryAsync(id);
                Snackbar.Add("Delete successful!", Severity.Success);
            }
            catch (Exception e)
            {
                Snackbar.Add($"Error deleting Category!: {e.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
            
        }
            
        await GetCategoriesAsync();

    }

    private void OnView(int id)
    {
        Navigation.NavigateTo($"/CategoryEdit/{id}/0");
    }

    private void OnAdd()
    {
        Navigation.NavigateTo("/CategoryEdit/0/2");
    }

    private async Task GetCategoriesAsync()
    {
        try
        {
            _isLoading = true;
            Categories = await Integration.GetCategoriesAsync();
            StateHasChanged();
        }
        catch (Exception e)
        {
            Snackbar.Add($"Unable to load Categories! {e.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }
}