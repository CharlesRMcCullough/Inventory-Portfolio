using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace InventoryClient.Components.Pages.Categories;

public partial class CategoryList : ComponentBase
{
    private IEnumerable<CategoryListViewModel> _categories = new List<CategoryListViewModel>();
    
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
            await Integration.DeleteCategoryAsync(id);
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
        _categories = await Integration.GetCategoriesAsync();
        StateHasChanged();
    }
}