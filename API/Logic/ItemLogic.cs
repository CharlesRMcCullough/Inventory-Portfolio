using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
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
        if (id == 0)
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
                    CheckInDate = i.CheckInDate,
                    CheckOutDate = i.CheckOutDate,
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
                CheckInDate = i.CheckInDate,
                CheckOutDate = i.CheckOutDate,
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
        
        var x = await context.Item
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
                CheckInDate = i.CheckInDate,
                CheckOutDate = i.CheckOutDate,
                CategoryId = p.CategoryId,
                Notes = i.Notes ?? string.Empty,
                CategoryName = (p.Category != null) ? p.Category.Name : string.Empty,
                MakeId = p.MakeId,
                MakeName = (p.Make != null) ? p.Make.Name : string.Empty,
                ModelId = p.ModelId,
                ModelName = (p.Model != null) ? p.Model.Name : string.Empty
            })
            .FirstOrDefaultAsync();

        return x;
    }
    
    public async Task<ItemDto> CreateItemAsync(ItemDto itemDto)
    {
        var itemToCreate = mapper.Map<Item>(itemDto);
        var createdItem = await context.Item.AddAsync(itemToCreate);
        await context.SaveChangesAsync();
        return mapper.Map<ItemDto>(createdItem.Entity);
        
    }
    
    public async Task<ItemDto> UpdateItemAsync(ItemDto itemDto)
    {
        var response = await context.Item.FindAsync(itemDto.Id);

        if (response == null)
        {
            throw new CustomExceptions.NotFoundException($"Item with id {itemDto.Id} was not found");
        }

        response.Id = itemDto.Id;
        response.SerialNumber = itemDto.SerialNumber;
        response.TagId = itemDto.TagId;
        response.ProductId = itemDto.ProductId;
        response.CheckOutDate = itemDto.CheckOutDate == null ? null : ToUtc(itemDto.CheckOutDate);
        response.CheckInDate = itemDto.CheckInDate == null ? null : ToUtc(itemDto.CheckInDate);
        response.Status = itemDto.Status;
        response.Price = itemDto.Price;
        response.Notes = itemDto.Notes;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var a = ex.Message;
        }

        return mapper.Map<ItemDto>(response);

    }
    public async Task DeleteItemAsync(int id)
    {
        var response = await context.Item.Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (response == null)
        {
            throw new CustomExceptions.NotFoundException($"Item with id {id} was not found");
        }
            
        context.Item.Remove(response);
        await context.SaveChangesAsync();
    }
    
    private DateTime? ToUtc(DateTime? date) => date?.ToUniversalTime();
}