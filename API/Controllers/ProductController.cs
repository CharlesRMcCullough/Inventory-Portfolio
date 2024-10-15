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
    
    [HttpGet("byCategory/{id:int}")]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByCategoryAsync(int id)
    {
        try
        {
            return Ok(await logic.GetProductsByCategoryAsync(id));
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto?>> GetProductById(int id)
    {
        try
        {
            var product = await logic.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error retrieving product {id} from the database! {ex.Message}");
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

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProductAsync([FromBody] ProductDto? requestBody)
    {
        try
        {
            if (requestBody == null)
            {
                return BadRequest();
            }

            var createdProduct = await logic.CreateProductAsync(requestBody);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = createdProduct.Id },
                createdProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Error creating new product record! {ex.Message}");
        }
    }

    [HttpPut]
    public async Task<ActionResult<ProductDto>> UpdateProduct([FromBody] ProductDto? productDto)
    {
        try
        {
            if (productDto == null)
                return BadRequest();

            var updatedProduct = await logic.UpdateProductAsync(productDto);

            if (updatedProduct == null) return NotFound($"Product with name {productDto.Name} was not found");

            return updatedProduct;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating product record!");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        try
        {
            await logic.DeleteProductsAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting product record!");
        }
    }


}