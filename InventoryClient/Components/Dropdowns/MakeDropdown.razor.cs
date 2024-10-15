using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace InventoryClient.Components.Dropdowns;

public partial class MakeDropdown : ComponentBase
{
    [Parameter] public bool Disabled { get; set; } = true;

    [Parameter] public int SelectedIndex { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public int CategoryId { get; set; }

    [Parameter] public bool Search { get; set; }

    [Parameter] public EventCallback<int> OnMakeChanged { get; set; }

    private IEnumerable<DropdownViewModel>? Makes { get; set; }
    private int _categoryId;

    protected override async Task OnInitializedAsync()
    {
        await GetMakesAsync();
        StateHasChanged();
    }


    protected override async Task OnParametersSetAsync()
    {
        if (CategoryId != _categoryId)
        {
            _categoryId = CategoryId;
            await GetMakesAsync();
        }

    }

    private async Task GetMakesAsync()
    {
        Makes = await Integration.GetMakesForDropdownsAsync(CategoryId);
        StateHasChanged();
    }

    private void OnSelectChanged(int makeId)
    {
        OnMakeChanged.InvokeAsync(makeId);
        SelectedIndex = makeId;
    }
}