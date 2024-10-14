using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using API.Entities;

namespace InventoryClient.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    [Required]
    [Range(0.01, 5000.00)]
    public decimal Price { get; set; }
    public string? Notes { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    [Required]
    [Range(0.01, 1000)]
    public int MakeId { get; set; }
    public string? MakeName { get; set; }
    [Required]
    public int ModelId { get; set; }
    public string? ModelName { get; set; }
    public bool Status { get; set; }

}