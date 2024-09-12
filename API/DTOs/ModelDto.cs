namespace API.DTOs;

public class ModelDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte Status { get; set; }
}