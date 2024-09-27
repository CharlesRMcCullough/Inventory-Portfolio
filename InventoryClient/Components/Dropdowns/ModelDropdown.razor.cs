using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace InventoryClient.Components.Dropdowns;

public partial class ModelDropdown : ComponentBase
{
    [Parameter]
    public bool Disabled { get; set; } = true;

    [Parameter]
    public int SelectedIndex { get; set;}

    [Parameter] public bool Required { get; set; } 
    
    [Parameter]
    public EventCallback<int> OnModelChanged { get; set; }
    
    private IEnumerable<DropdownViewModel>? Models { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetModels();

    }

    private async Task GetModels()
    {
        Models = await Integration.GetModelsForDropdownsAsync();
    }

    private void OnSelectChanged(int modelId)
    {
        OnModelChanged.InvokeAsync(modelId);
        SelectedIndex = modelId;
    }
}