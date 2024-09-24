using API.Entities;
using InventoryClient.Integrations;
using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace InventoryClient.Components;

public partial class CategoryDropdown : ComponentBase
{
    private int _ddValue;
    
    private IEnumerable<DropdownViewModel>? Categories { get; set; }

    protected override async Task OnInitializedAsync()
    {
           await GetCategories(); 

        // Categories = new List<DropdownViewModel>
        // {
        //     new DropdownViewModel { Id = 1, Name = "Category 1" },
        //     new DropdownViewModel { Id = 2, Name = "Category 2" },
        //     new DropdownViewModel { Id = 1, Name = "Category 3" },
        //     new DropdownViewModel { Id = 1, Name = "Category 4" },

        // };

    }

    private async Task GetCategories()
    {
        Categories = await Integration.GetCategoriesForDropdownsAsync();
    }
}