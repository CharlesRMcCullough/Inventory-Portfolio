using API.DTOs;
using API.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers;

[ApiController]
[Route("api/items")]
public class ItemController(IItemLogic logic) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ItemDto>>> GetItems()
    {
        try
        {
            return Ok(await logic.GetItemsAsync());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving Items from the database!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Items from the database!");
        }
    }
    
    [HttpGet("{productId:int}")]
    public async Task<ActionResult<IReadOnlyList<ItemDto>>> GetItemsByProductId(int productId)
    {
        try
        {
            var items = await logic.GetItemsByProductIdAsync(productId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error retrieving items by product id from the database: {ex.Message}";
            Log.Error(ex, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
        }
    }
    
    
    [HttpGet("item/{id:int}")]
    public async Task<ActionResult<ItemDto?>> GetItemByIdAsync(int itemId)
    {
        try
        {
            return Ok(await logic.GetItemByIdAsync(itemId));
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to retrieve item by ID: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve item");
        }
    }
}