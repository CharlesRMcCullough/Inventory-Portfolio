using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Model
{
    public int Id { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    [StringLength(255)]
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Status { get; set; }
    
    public int MakeId { get; set; }
    public Make? Make { get; set; } = null!;    // Navigation Property
}