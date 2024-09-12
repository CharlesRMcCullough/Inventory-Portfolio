using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;

namespace API.Logic;

public class ProductLogic(InventoryDbContext context, IMapper mapper) : IProductLogic
{
    public async Task<ActionResult<List<ProductDto>>> GetProductsAsync()
    {
        return await context.Products.Where(p => p.Status == 1)
            .OrderBy(p => p.Name)
            .Include(c => c.Category)
            .Include(m => m.Model)
            .Include(ma => ma.Make)
            .Select(p => mapper.Map<ProductDto>(p))
            .ToListAsync();
    }
    
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
    {
            var response = await context.Products.Where(p => p.Id == id && p.Status == 1)
                .Include(c => c.Category)
                .Include(m => m.Model)
                .Include(ma => ma.Make)
                .Select(p => mapper.Map<ProductDto>(p))
                .FirstOrDefaultAsync();

            return response ?? new ProductDto();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }


}