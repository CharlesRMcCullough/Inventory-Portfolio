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
        return await context.Category.Where(p => p.Status == true)
            .OrderBy(p => p.Name)
            .Select(p => mapper.Map<CategoryDto>(p))
            .ToListAsync();
    }
    
    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var response = await context.Category.Where(p => p.Id == id && p.Status == true)
            .Select(p => mapper.Map<CategoryDto>(p))
            .FirstOrDefaultAsync();
        return response ?? new CategoryDto();
    }

    public async Task<List<DropdownDto>> GetCategoriesForDropdownAsync()
    {
        return await context.Category.Where(c => c.Status == true)
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

        var recordToCreate = mapper.Map<Category>(categoryDto);
        var result = await context.Category.AddAsync(recordToCreate);
        await context.SaveChangesAsync();
        
        return mapper.Map<CategoryDto>(result.Entity);
    }
    
    public async Task<CategoryDto?> UpdateCategoryAsync(CategoryDto categoryDto)
    {
        var response = await context.Category.FindAsync(categoryDto.Id);

        if (response == null) return null;
        
        response.Id = categoryDto.Id;
        response.Name = categoryDto.Name;
        response.Description = categoryDto.Description;;
        response.Status = categoryDto.Status;
        
        await context.SaveChangesAsync();
        
        return mapper.Map<CategoryDto>(response);
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