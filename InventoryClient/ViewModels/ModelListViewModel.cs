using System.ComponentModel.DataAnnotations;

namespace InventoryClient.ViewModels;

public class ModelListViewModel
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }

    public bool Status { get; set; }
}