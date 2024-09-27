using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace InventoryClient.Components.Dropdowns;

public partial class MakeDropdown : ComponentBase
{
    [Parameter] public bool Disabled { get; set; } = true;

    [Parameter] public int SelectedIndex { get; set; }

    [Parameter]
    public bool Required { get; set; } = false;

    [Parameter] public EventCallback<int> OnMakeChanged { get; set; }

    private IEnumerable<DropdownViewModel>? Makes { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetMakes();
    }

    private async Task GetMakes()
    {
        Makes = await Integration.GetMakesForDropdownsAsync();
    }

    private void OnSelectChanged(int categoryId)
    {
        OnMakeChanged.InvokeAsync(categoryId);
        SelectedIndex = categoryId;
    }
}