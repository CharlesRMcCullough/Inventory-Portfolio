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
    public async Task<ActionResult<ItemDto?>> GetItemById(int id)
    {
        try
        {
            return Ok(await logic.GetItemByIdAsync(id));
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to retrieve item by ID: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve item");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync([FromBody] ItemDto? requestBody)
    {
        try
        {
            if (requestBody == null)
            {
                return BadRequest();
            }

            var createdItem = await logic.CreateItemAsync(requestBody);

            return CreatedAtAction(
                nameof(GetItemById),
                new { id = createdItem.Id },
                createdItem);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Error creating new item record! {ex.Message}");
        }
    }

    [HttpPut]
    public async Task<ActionResult<ItemDto>> UpdateItem([FromBody] ItemDto? itemDto)
    {
        try
        {
            if (itemDto == null)
                return BadRequest();

            var updatedItem = await logic.UpdateItemAsync(itemDto);

            if (updatedItem == null) return NotFound($"Item with name {itemDto.Name} was not found");

            return updatedItem;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating item record!");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        try
        {
            await logic.DeleteItemAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting item record!");
        }
    }
}