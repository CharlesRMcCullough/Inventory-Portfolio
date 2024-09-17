using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace InventoryClient.Components.Pages.Categories;

public partial class CategoryEdit : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    [Parameter]
    public int Mode { get; set; }

    private bool IsView => Mode == 0 ? true : false;
    private bool IsAdd => Mode == 3 ? true : false;
    private bool IsDisabled => Mode == 0 ? true : false;
    
    private CategoryListViewModel model = new();

    protected override async Task OnParametersSetAsync()
    {
        if (!IsAdd)
        {
            var category = await Integration.GetCategoryByIdAsync(Id);
            model.Id = category.Id;
            model.Name = category.Name;
            model.Description = category.Description;
            model.Status = category.Status;
        }
    }

    private async Task OnValidSubmit(EditContext context)
    {
        if (IsAdd)
        {
            var response = await Integration.CreateCategoryAsync(model);
        }
        else
        {
            var response = await Integration.UpdateCategoryAsync(model);
        }
        
        Navigation.NavigateTo("/Categories");
    }

    private void OnCancel()
    {
        Navigation.NavigateTo("/Categories");
    }
}