using API.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Logic.Interfaces;
using Serilog;
using API.Entities;
using AutoMapper.QueryableExtensions;

namespace API.Logic;

public class ProductLogic(InventoryDbContext context, IMapper mapper) : IProductLogic
{

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        try
        {
            return await context.Products
                .Where(p => p.Status == 1)
                .Include(c => c.Category)
                .Include(m => m.Model)
                .Include(ma => ma.Make)
                .AsNoTracking()
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error getting products - GetProductsAsync {ex.Message}");
            throw;
        }
    }
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        try
        {
            return await context.Products
                .Where(p => p.Id == id && p.Status == 1)
                .Include(c => c.Category)
                .Include(m => m.Model)
                .Include(ma => ma.Make)
                .Select(p => mapper.Map<ProductDto>(p))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error getting product by id - GetProductByIdAsync {ex.Message}");
            return null;

        }
    }

    public async Task<List<DropdownDto>?> GetProductsForDropdownAsync()
    {
        try
        {
            return await (from product in context.Products
                          where product.Status == 1
                          orderby product.Name
                          select new DropdownDto
                          {
                              Id = product.Id,
                              Name = product.Name
                          }).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error getting products for dropdown - GetProductsForDropdownAsync {ex.Message}");
            return null;
        }
    }

    public async Task<ProductDto?> CreateProductAsync(ProductDto productDto)
    {
        try
        {
            var recordToCreate = mapper.Map<Product>(productDto);
            var result = await context.Products.AddAsync(recordToCreate);
            await context.SaveChangesAsync();
            return mapper.Map<ProductDto>(result.Entity);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error creating product for - CreateProductAsync {ex.Message}");
            return null;
        }
    }

    public async Task<ProductDto?> UpdateProductAsync(ProductDto productDto)
    {
        try
        {
            var response = await context.Products.FindAsync(productDto.Id);

            if (response == null) return null;

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
        catch (Exception ex)
        {
            Log.Error(ex, $"Error creating product for - CreateProductAsync {ex.Message}");
            return null;
        }
    }
    public async Task DeleteProductsAsync(int id)
    {
        try
        {
            var response = await context.Products.Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (response != null)
            {
                context.Products.Remove(response);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error deleting product for - DeleteProductsAsync {ex.Message}");
        }
    }
}




