using API.DTOs;
using API.Logic;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController(ICategoryLogic logic) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        try
        {
            return Ok(await logic.GetCategoriesAsync());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Categories from the database!");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto?>> GetCategoryById(int id)
    {
        try
        {
            var result = await logic.GetCategoryByIdAsync(id);

            if (result == null) return NotFound();

            return result;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Category from the database!");
        }
    }

    [HttpGet("dropdowns")]
    public async Task<ActionResult<List<DropdownDto>>> GetCategoryDropdownList()
    {
        try
        {
            return await logic.GetCategoriesForDropdownAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Category from the database!");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto? categoryDto)
    {
        try
        {
            if (categoryDto == null)
                return BadRequest();

            var createdCategory = await logic.CreateCategoryAsync(categoryDto);

            return CreatedAtAction(nameof(GetCategoryById),
                new { id = createdCategory.Id }, createdCategory);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new Category record!");
        }
    }

    [HttpPut]
    public async Task<ActionResult<CategoryDto>> UpdateCategory([FromBody] CategoryDto? categoryDto)
    {
        try
        {
            if (categoryDto == null)
                return BadRequest();

            var updatedCategory = await logic.UpdateCategoryAsync(categoryDto);

            if (updatedCategory == null) return NotFound($"Category with name {categoryDto.Name} was not found");

            return updatedCategory;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating Category record!");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        try
        {
            await logic.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting Category record!");
        }
    }
}