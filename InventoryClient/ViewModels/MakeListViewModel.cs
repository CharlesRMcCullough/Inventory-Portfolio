using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryClient.ViewModels;

public class MakeListViewModel
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public bool Status { get; set; }
}
