namespace API.DTOs;

public class ItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? SerialNumber { get; set; }
    public string? TagId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? Notes { get; set; }
    public string? History { get; set; }
    public string? Location { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int ModelId { get; set; }
    public string? ModelName { get; set; }
    public int MakeId { get; set; }
    public string? MakeName { get; set; }
    public bool Status { get; set; }
}