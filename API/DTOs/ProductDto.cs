using API.Entities;

namespace API.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? Notes { get; set; }
    public Category? Category { get; set; }
    public Model? Model { get; set; }
    public Make? Make { get; set; }
    public byte Status { get; set; }
}