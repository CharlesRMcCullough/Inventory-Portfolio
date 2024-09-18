using InventoryClient.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace InventoryClient.Components.Pages.Categories;

public partial class CategoryEdit : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Parameter] public int Mode { get; set; }

    private bool IsView => Mode == 0 ? true : false;
    private bool IsAdd => Mode == 2 ? true : false;
    private bool IsDisabled => Mode == 0 ? true : false;

    private CategoryListViewModel model = new();
    private string _categoryPrompt = string.Empty;

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
        
        _categoryPrompt = IsAdd ? "Category Add" : $"Category: {model.Name}";
    }

    private async Task OnValidSubmit(EditContext context)
    {
        if (IsAdd)
        {
            model.Id = 0;
            await Integration.CreateCategoryAsync(model);
            Navigation.NavigateTo("/categories");
        }
        else
        {
            await Integration.UpdateCategoryAsync(model);
        }

        Navigation.NavigateTo("/Categories");
    }

    private void OnCancel()
    {
        Navigation.NavigateTo("/Categories");
    }
}