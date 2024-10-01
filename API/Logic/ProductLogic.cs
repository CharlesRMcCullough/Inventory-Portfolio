using API.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Logic.Interfaces;
using Serilog;

namespace API.Logic;

public class ProductLogic(InventoryDbContext context, IMapper mapper) : IProductLogic
{
    public async Task<List<ProductDto>?> GetProductsAsync()
    {
        try
        {
            var product = await context.Products
                .Where(p => p.Status == 1)
                .Include(c => c.Category)
                .Include(m => m.Model)
                .Include(ma => ma.Make)
                .OrderBy(p => p.Name)
                .ToListAsync();
            
            var products = mapper.Map<List<ProductDto>>(product);
            return products;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error getting products - GetProductsAsync {ex.Message}");
            return null;
        }
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        try
        {
            return await context.Products
                .Include(c => c.Category)
                .Include(m => m.Model)
                .Include(ma => ma.Make)
                .Select(p => mapper.Map<ProductDto>(p))
                .FirstOrDefaultAsync(p => p.Id == id && p.Status == 1);
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
}




