using API.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Logic.Interfaces;

using API.Entities;
using API.Exceptions;

namespace API.Logic;

public class ProductLogic(InventoryDbContext context, IMapper mapper) : IProductLogic
{
    public async Task<List<ProductDto>> GetProductsAsync()
    {
            return await context.Products
                .Include(p => p.Item) 
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    MakeId = p.MakeId,
                    MakeName = p.Make != null ? p.Make.Name : string.Empty,
                    ModelId = p.ModelId,
                    ModelName = p.Model != null ? p.Model.Name : string.Empty,
                    Status = p.Status,
                    Quantity = context.Item.Count(i => i.ProductId == p.Id && i.Status),
                    AvailableQuantity = context.Item.Count(i => i.ProductId == p.Id && i.Status && i.CheckOutDate == null)
                })
                .AsNoTracking()
                .ToListAsync();  
    }
    
    public async Task<List<ProductDto>> GetProductsByCategoryAsync(int id)
    {
        if (id == 0)
        {
            return await context.Products
                .Include(p => p.Item)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    MakeId = p.MakeId,
                    MakeName = p.Make != null ? p.Make.Name : string.Empty,
                    ModelId = p.ModelId,
                    ModelName = p.Model != null ? p.Model.Name : string.Empty,
                    Status = p.Status,
                    Quantity = context.Item.Count(i => i.ProductId == p.Id && i.Status),
                    AvailableQuantity = context.Item.Count(i => i.ProductId == p.Id && i.Status && i.CheckOutDate == null)
                })
                .AsNoTracking()
                .ToListAsync();
        }
        else
        {
            return await context.Products
                .Include(p => p.Item)
                .Where(p => p.CategoryId == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    MakeId = p.MakeId,
                    MakeName = p.Make != null ? p.Make.Name : string.Empty,
                    ModelId = p.ModelId,
                    ModelName = p.Model != null ? p.Model.Name : string.Empty,
                    Status = p.Status,
                    Quantity = context.Item.Count(i => i.ProductId == p.Id && i.Status),
                    AvailableQuantity = context.Item.Count(i => i.ProductId == p.Id && i.Status && i.CheckOutDate == null)
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
            return await context.Products
                .Include(p => p.Item) 
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    MakeId = p.MakeId,
                    MakeName = p.Make != null ? p.Make.Name : string.Empty,
                    ModelId = p.ModelId,
                    ModelName = p.Model != null ? p.Model.Name : string.Empty,
                    Status = p.Status,
                    Quantity = context.Item.Count(i => i.ProductId == p.Id && i.Status),
                    AvailableQuantity = context.Item.Count(i => i.ProductId == p.Id && i.Status && i.CheckOutDate == null)
                })
                .FirstOrDefaultAsync();  
    }

    public async Task<List<DropdownDto>> GetProductsForDropdownAsync()
    {
            return await (from product in context.Products
                          where product.Status == true
                          orderby product.Name
                          select new DropdownDto
                          {
                              Id = product.Id,
                              Name = product.Name
                          }).ToListAsync();
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
            var productToCreate = mapper.Map<Product>(productDto);
            var createdProduct = await context.Products.AddAsync(productToCreate);
            await context.SaveChangesAsync();
            return mapper.Map<ProductDto>(createdProduct.Entity);
    }

    public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
    {
            var response = await context.Products.FindAsync(productDto.Id);

            if (response == null)
            {
                throw new CustomExceptions.NotFoundException($"Product with id {productDto.Id} was not found");
            }

            response.Id = productDto.Id;
            response.Name = productDto.Name;
            response.Description = productDto.Description;
            response.CategoryId = productDto.CategoryId;
            response.MakeId = productDto.MakeId;
            response.ModelId = productDto.ModelId;
            response.Price = productDto.Price;
            response.Quantity = productDto.Quantity;
            response.Status = productDto.Status;
            response.Notes = productDto.Notes;

            await context.SaveChangesAsync();

            return mapper.Map<ProductDto>(response);

    }
    public async Task DeleteProductsAsync(int id)
    {
            var response = await context.Products.Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (response == null)
            {
                throw new CustomExceptions.NotFoundException($"Product with id {id} was not found");
            }
            
            context.Products.Remove(response);
            await context.SaveChangesAsync();
    }
}




