using API.Entities;
using InventoryClient.Integrations;
using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace InventoryClient.Components.Dropdowns;
// https://try.mudblazor.com/snippet/wOGwuMwGcrxmCbKy
public partial class CategoryDropdown : ComponentBase
{
    [Parameter]
    public bool Disabled { get; set; } = true;

    [Parameter]
    public int SelectedIndex { get; set;}

    [Parameter] 
    public bool Required { get; set; } = false;
    
    [Parameter]
    public bool Search { get; set; }
    
    
    [Parameter]
    public EventCallback<int> OnCategoryChanged { get; set; }
    
    private IEnumerable<DropdownViewModel>? Categories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetCategories();

    }

    private async Task GetCategories()
    {
        Categories = await Integration.GetCategoriesForDropdownsAsync();
        StateHasChanged();
    }

    private void OnSelectChanged(int categoryId)
    {
        OnCategoryChanged.InvokeAsync(categoryId);
        SelectedIndex = categoryId;
    }
}