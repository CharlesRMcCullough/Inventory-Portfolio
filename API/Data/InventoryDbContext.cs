using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Make> Make { get; set; }
    public new DbSet<Model> Model { get; set; }
    public DbSet<Category> Category { get; set; }
}