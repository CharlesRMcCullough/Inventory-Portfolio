using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Logic;

public interface ICategoryLogic
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<List<DropdownDto>> GetCategoriesForDropdownAsync();
    Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
    Task<CategoryDto?> UpdateCategoryAsync(CategoryDto categoryDto);
    Task DeleteCategoryAsync(int id);
}