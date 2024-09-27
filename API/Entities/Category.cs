using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Category
{
    public int Id { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    [StringLength(255)]
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public byte Status { get; set; }
    
    public Category? ParentCategory { get; set; }
    
    public bool CategoryHasName() => Name is not null;
}