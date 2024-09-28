using API.DTOs;
using API.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductLogic logic) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProductsAsync()
    {
        try
        {
            return Ok(await logic.GetProductsAsync());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                "Error retrieving data from the database");
        }
    }
    
    public async Task<ActionResult<ProductDto?>> GetProductByIdAsync(int id)
    {
        try
        {
            var result = await logic.GetProductByIdAsync(id);

            if (result == null) return NotFound();

            return result;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving product from the database!");
        }
    }
    
    [HttpGet("dropdowns")]
    public async Task<ActionResult<List<DropdownDto>?>> GetProductDropdownListAsync()
    {
        try
        {
            return await logic.GetProductsForDropdownAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving products for dropdown list from the database!");
        }
    }
    

}