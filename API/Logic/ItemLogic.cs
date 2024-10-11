using API.Data;
using API.DTOs;
using API.Entities;
using API.Logic.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Logic;

public class ItemLogic(InventoryDbContext context, IMapper mapper) : IItemLogic
{
    public async Task<IReadOnlyList<ItemDto>> GetItemsAsync()
    {
            return await context.Item
                .Where(i => i.Status)
                .Join(context.Products, i => i.ProductId, p => p.Id, (i, p) => new ItemDto
                {
                    Id = i.Id,
                    ProductId = p.Id,
                    SerialNumber = i.SerialNumber,
                    TagId = i.TagId,
                    Name = p.Name ?? string.Empty,
                    Description = p.Description ?? string.Empty,
                    Status = i.Status,
                    Price = i.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = (p.Category != null) ? p.Category.Name : string.Empty,
                    MakeId = p.MakeId,
                    MakeName = (p.Make != null) ? p.Make.Name : string.Empty,
                    ModelId = p.ModelId,
                    ModelName = (p.Model != null) ? p.Model.Name : string.Empty
                })
                .OrderBy(i => i.Name)
                .ToListAsync();
    }
    
    public async Task<IReadOnlyList<ItemDto>> GetItemsByProductIdAsync(int id)
    {
        return await context.Item
            .Where(i => i.ProductId == id && i.Status)
            .Join(context.Products, i => i.ProductId, p => p.Id, (i, p) => new ItemDto
            {
                Id = i.Id,
                ProductId = p.Id,
                SerialNumber = i.SerialNumber,
                TagId = i.TagId,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Status = i.Status,
                Price = i.Price,
                CategoryId = p.CategoryId,
                CategoryName = (p.Category != null) ? p.Category.Name : string.Empty,
                MakeId = p.MakeId,
                MakeName = (p.Make != null) ? p.Make.Name : string.Empty,
                ModelId = p.ModelId,
                ModelName = (p.Model != null) ? p.Model.Name : string.Empty
            })
            .OrderBy(i => i.Name)
            .ToListAsync();
    }
    
    public async Task<ItemDto?> GetItemByIdAsync(int id)
    {
        return await context.Item
            .Where(i => i.Id == id && i.Status)
            .Join(context.Products, i => i.ProductId, p => p.Id, (i, p) => new ItemDto
            {
                Id = i.Id,
                ProductId = p.Id,
                SerialNumber = i.SerialNumber,
                TagId = i.TagId,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Status = i.Status,
                Price = i.Price,
                CategoryId = p.CategoryId,
                CategoryName = (p.Category != null) ? p.Category.Name : string.Empty,
                MakeId = p.MakeId,
                MakeName = (p.Make != null) ? p.Make.Name : string.Empty,
                ModelId = p.ModelId,
                ModelName = (p.Model != null) ? p.Model.Name : string.Empty
            })
            .FirstOrDefaultAsync();
    }
}