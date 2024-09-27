namespace API.DTOs;

public class ModelDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int MakeId { get; set; }
    public  string? MakeName { get; set; }
    public byte Status { get; set; }
}