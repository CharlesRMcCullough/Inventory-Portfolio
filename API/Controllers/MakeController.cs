using API.DTOs;
using API.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/makes")]
public class MakeController(IMakeLogic logic) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MakeDto>>> GetMakesAsync()
    {
        try
        {
            return Ok(await logic.GetMakesAsync());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Makes from the database!");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MakeDto?>> GetMakeById(int id)
    {
        try
        {
            var result = await logic.GetMakeByIdAsync(id);

            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Make record from the database!");
        }
    }
    
    [HttpGet("byCategory/{categoryId:int}")]
    public async Task<ActionResult<List<MakeDto?>>> GetMakesByCategoryId(int categoryId)
    {
        try
        {
            var result = await logic.GetMakesByCategoryIdAsync(categoryId);

            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Make records by category from the database!");
        }
    }
    
    [HttpGet("dropdowns")]
    public async Task<ActionResult<List<DropdownDto>>> GetMakeDropdownList()
    {
        try
        {
            return await logic.GetMakesForDropdownAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving Make data from the database!");
        }
    }

    [HttpPost]
        public async Task<ActionResult<MakeDto>> CreateMakeAsync([FromBody] MakeDto? makeDto)
        {
            try
            {
                if (makeDto == null)
                    return BadRequest();

                var createdMake = await logic.CreateMakeAsync(makeDto);

                return CreatedAtAction(nameof(GetMakeById),
                    new { id = createdMake.Id }, createdMake);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Make record!");
            }
        }

        [HttpPut]
        public async Task<ActionResult<MakeDto>> UpdateMake([FromBody] MakeDto? makeDto)
        {
            try
            {
                if (makeDto == null)
                    return BadRequest();

                var updatedMake = await logic.UpdateMakeAsync(makeDto);

                if (updatedMake == null) return NotFound($"Make with name {makeDto.Name} was not found");

                return updatedMake;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating Make record!");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMake(int id)
        {
            try
            {
                await logic.DeleteMakeAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting Make record!");
            }
        }
    }