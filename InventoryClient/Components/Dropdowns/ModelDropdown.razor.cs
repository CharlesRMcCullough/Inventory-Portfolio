using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace InventoryClient.Components.Dropdowns;

public partial class ModelDropdown : ComponentBase
{
    [Parameter] public bool Disabled { get; set; } = true;

    [Parameter] public int SelectedIndex { get; set; }
    [Parameter] public int MakeId { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public bool Search { get; set; }

    [Parameter] public EventCallback<int> OnModelChanged { get; set; }

    private IEnumerable<DropdownViewModel>? Models { get; set; }
    private int _makeId;

    protected override async Task OnInitializedAsync()
    {
        await GetModelsAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (MakeId != _makeId)
        {
            _makeId = MakeId;
            await GetModelsAsync();
        }
    }

    private async Task GetModelsAsync()
    {
        Models = await Integration.GetModelsForDropdownsAsync(MakeId);
        StateHasChanged();
    }

    private void OnSelectChanged(int modelId)
    {
        OnModelChanged.InvokeAsync(modelId);
        SelectedIndex = modelId;
    }
}