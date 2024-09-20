using API.Data;
using API.DTOs;
using API.Entities;
using API.Logic.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Logic;

public class ModelLogic(InventoryDbContext context, IMapper mapper) : IModelLogic
{
     public async Task<List<ModelDto>> GetModelsAsync()
    {
        return await context.Model.Where(p => p.Status == 1)
            .OrderBy(p => p.Name)
            .Select(p => mapper.Map<ModelDto>(p))
            .ToListAsync();
    }
    
    public async Task<ModelDto?> GetModelByIdAsync(int id)
    {
        var response = await context.Model.Where(p => p.Id == id && p.Status == 1)
            .Select(p => mapper.Map<ModelDto>(p))
            .FirstOrDefaultAsync();
        return response ?? new ModelDto();
    }

    public async Task<List<DropdownDto>> GetModelsForDropdownAsync()
    {
        return await context.Model.Where(c => c.Status == 1)
            .OrderBy(c => c.Name)
            .Select(c => new DropdownDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
            .ToListAsync();
    }
    
    public async Task<ModelDto> CreateModelAsync(ModelDto modelDto)
    {

        var recordToCreate = mapper.Map<Model>(modelDto);
        var result = await context.Model.AddAsync(recordToCreate);
        await context.SaveChangesAsync();
        
        return mapper.Map<ModelDto>(result.Entity);
    }
    
    public async Task<ModelDto?> UpdateModelAsync(ModelDto modelDto)
    {
        var response = await context.Model.FindAsync(modelDto.Id);

        if (response == null) return null;
        
        response.Id = modelDto.Id;
        response.Name = modelDto.Name;
        response.Description = modelDto.Description;
        response.Status = modelDto.Status;
        
        await context.SaveChangesAsync();
        
        return mapper.Map<ModelDto>(response);
    }
    public async Task DeleteModelAsync(int id)
    {
        var response = await context.Model.Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (response != null)
        {
            context.Model.Remove(response);
            await context.SaveChangesAsync();
        }
    }
}