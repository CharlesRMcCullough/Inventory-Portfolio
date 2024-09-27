using API.DTOs;
using API.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/models")]
public class ModelController(IModelLogic logic) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ModelDto>>> GetModelsAsync()
    {
        try
        {
            return Ok(await logic.GetModelsAsync());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Model from the database!");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ModelDto?>> GetModelById(int id)
    {
        try
        {
            var result = await logic.GetModelByIdAsync(id);

            if (result == null) return NotFound();

            return result;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Model data from the database!");
        }
    }
    
    [HttpGet("byMake/{makeId:int}")]
    public async Task<ActionResult<List<ModelDto?>>> GetModelsByMakeId(int makeId)
    {
        try
        {
            var result = await logic.GetModelsByMakeIdAsync(makeId);

            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Make records by makes from the database!");
        }
    }

    [HttpGet("dropdowns")]
    public async Task<ActionResult<List<DropdownDto>?>> GetModelDropdownListAsync()
    {
        try
        {
            return Ok(await logic.GetModelsForDropdownAsync());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Model record from the database!");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ModelDto>> CreateModelAsync([FromBody] ModelDto? modelDto)
    {
        try
        {
            if (modelDto == null)
                return BadRequest();

            var createdModel = await logic.CreateModelAsync(modelDto);
            
            if (createdModel == null) return BadRequest();
            
            return CreatedAtAction(nameof(GetModelById),
                new { id = createdModel.Id }, createdModel);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new Model record!");
        }
    }

    [HttpPut]
    public async Task<ActionResult<ModelDto>> UpdateModelAsync([FromBody] ModelDto? modelDto)
    {
        try
        {
            if (modelDto == null)
                return BadRequest();

            var updatedModel = await logic.UpdateModelAsync(modelDto);

            if (updatedModel == null) return NotFound($"Model with name {modelDto.Name} was not found");

            return updatedModel;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating Model record!");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteModel(int id)
    {
        try
        {
            await logic.DeleteModelAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting Model record!");
        }
    }
}