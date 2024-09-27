using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Make
{
    public int Id { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    [StringLength(255)]
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public byte Status { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Model? Model { get; set; } = null!; // Navigation Property
}