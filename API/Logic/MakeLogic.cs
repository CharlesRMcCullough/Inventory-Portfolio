using API.Data;
using API.DTOs;
using API.Entities;
using API.Logic.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Logic;

public class MakeLogic(InventoryDbContext context, IMapper mapper) : IMakeLogic
{
    public async Task<List<MakeDto>> GetMakesAsync()
    {
        return await context.Make.Where(p => p.Status == 1)
            .OrderBy(p => p.Name)
            .Select(p => mapper.Map<MakeDto>(p))
            .ToListAsync();
    }
    
    public async Task<MakeDto?> GetMakeByIdAsync(int id)
    {
        var response = await context.Make.Where(p => p.Id == id && p.Status == 1)
            .Select(p => mapper.Map<MakeDto>(p))
            .FirstOrDefaultAsync();
        return response ?? new MakeDto();
    }
    
    public async Task<List<DropdownDto>> GetMakesForDropdownAsync()
    {
        return await context.Make.Where(c => c.Status == 1)
            .OrderBy(c => c.Name)
            .Select(c => new DropdownDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }
    
    public async Task<MakeDto> CreateMakeAsync(MakeDto makeDto)
    {

        var recordToCreate = mapper.Map<Make>(makeDto);
        var result = await context.Make.AddAsync(recordToCreate);
        await context.SaveChangesAsync();
        
        return mapper.Map<MakeDto>(result.Entity);
    }
    
    public async Task<MakeDto?> UpdateMakeAsync(MakeDto makeDto)
    {
        var response = await context.Make.Where(p => p.Id == makeDto.Id)
            .FirstOrDefaultAsync();

        if (response == null) return null;
        
        response.Id = makeDto.Id;
        response.Name = makeDto.Name;
        response.Description = makeDto.Description;
        response.Status = makeDto.Status;
        
        await context.SaveChangesAsync();
        
        return mapper.Map<MakeDto>(response);
    }
    public async Task DeleteMakeAsync(int id)
    {
        var response = await context.Make.Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (response != null)
        {
            context.Make.Remove(response);
            await context.SaveChangesAsync();
        }
    }
    
}