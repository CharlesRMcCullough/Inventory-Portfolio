using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Item
{
    public int Id { get; set; }
    [StringLength(255)]
    public string? SerialNumber { get; set; }
    [StringLength(255)]
    public string? TagId { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public decimal Price { get; set; }
    public string? Notes { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
}