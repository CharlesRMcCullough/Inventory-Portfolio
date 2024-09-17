using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Logic;

public class CategoryLogic(InventoryDbContext context, IMapper mapper) : ICategoryLogic
{
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        return await context.Category.Where(p => p.Status == 1)
            .OrderBy(p => p.Name)
            .Select(p => mapper.Map<CategoryDto>(p))
            .ToListAsync();
    }
    
    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var response = await context.Category.Where(p => p.Id == id && p.Status == 1)
            .Select(p => mapper.Map<CategoryDto>(p))
            .FirstOrDefaultAsync();
        return response ?? new CategoryDto();
    }

    public async Task<List<DropdownDto>> GetCategoriesForDropdownAsync()
    {
        return await context.Category.Where(c => c.Status == 1)
            .OrderBy(c => c.Name)
            .Select(c => new DropdownDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
            .ToListAsync();
    }
    
    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var newCategory = mapper.Map<Category>(categoryDto);
        
        context.Add(newCategory);
        var result = await context.SaveChangesAsync();
        
        return mapper.Map<CategoryDto>(result);
    }
    
    public async Task<CategoryDto?> UpdateCategoryAsync(CategoryDto categoryDto)
    {
        var response = await context.Category.Where(p => p.Id == categoryDto.Id)
            .FirstOrDefaultAsync();

        if (response == null) return null;
        
        response.Name = categoryDto.Name;
        response.Description = categoryDto.Description;;
        response.Status = categoryDto.Status;
        
        var createdCategory = await context.SaveChangesAsync();
        
        return mapper.Map<CategoryDto>(createdCategory);
    }
    public async Task DeleteCategoryAsync(int id)
    {
        var response = await context.Category.Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (response != null)
        {
            context.Category.Remove(response);
            await context.SaveChangesAsync();
        }
    }
}