using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace InventoryClient.Components.Dropdowns;

public partial class ProductDropdown : ComponentBase
{
    [Parameter]
    public bool Disabled { get; set; } = true;

    [Parameter]
    public int SelectedIndex { get; set;}

    [Parameter] public bool Required { get; set; } 
    [Parameter] public bool Search { get; set; }
    
    [Parameter]
    public EventCallback<int> OnProductChanged { get; set; }
    
    private IEnumerable<DropdownViewModel>? Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetProducts();

    }

    private async Task GetProducts()
    {
        Products = await Integration.GetProductsForDropdownsAsync();
        StateHasChanged();
    }

    private void OnSelectChanged(int productId)
    {
        OnProductChanged.InvokeAsync(productId);
        SelectedIndex = productId;
    }
}