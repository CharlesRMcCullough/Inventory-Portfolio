
namespace InventoryClient.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
    public decimal Price { get; set; }
    public string? Notes { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public int MakeId { get; set; }
    public string? MakeName { get; set; }
    public int ModelId { get; set; }
    public string? ModelName { get; set; }
    public bool Status { get; set; }

}