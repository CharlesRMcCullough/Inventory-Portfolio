using API.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Logic.Interfaces;

namespace API.Logic;

public class ProductLogic(InventoryDbContext context, IMapper mapper) : IProductLogic
{
    public async Task<List<ProductDto>?> GetProductsAsync()
    {
        return await context.Products.Where(p => p.Status == 1)
            .OrderBy(p => p.Name)
            .Include(c => c.Category)
            .Include(m => m.Model)
            .Include(ma => ma.Make)
            .Select(p => mapper.Map<ProductDto>(p))
            .ToListAsync();
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var response = await context.Products.Where(p => p.Id == id && p.Status == 1)
            .Include(c => c.Category)
            .Include(m => m.Model)
            .Include(ma => ma.Make)
            .Select(p => mapper.Map<ProductDto>(p))
            .FirstOrDefaultAsync();

        return response ?? new ProductDto();
    }

    public async Task<List<DropdownDto>?> GetProductsForDropdownAsync()
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
}




