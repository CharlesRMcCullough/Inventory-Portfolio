using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Product
{
    public int Id { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    [StringLength(255)]
    public string? Description { get; set; }
    [StringLength(100)]
    public string? SerialNumber { get; set; }
    [StringLength(255)]
    public string? TagId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    [StringLength(200)]
    public string? Notes { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public byte Status { get; set; }
    
    public Make? Make { get; set; }
    public Model? Model { get; set; }
    public Category? Category{ get; set; }
}