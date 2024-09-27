using API.Data;
using API.DTOs;
using API.Entities;
using API.Logic.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace API.Logic;

public class ModelLogic(InventoryDbContext context, IMapper mapper) : IModelLogic
{
    public async Task<List<ModelDto>?> GetModelsAsync()
    {
        try
        {
            return await (from model in context.Model
                join make in context.Make
                    on model.MakeId equals make.Id
                where model.Status == 1
                orderby model.Name
                select new ModelDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status,
                    MakeId = make.Id,
                    MakeName = make.Name
                }).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting models - GetModelsAsync");
            return null;
        }
    }

    public async Task<ModelDto?> GetModelByIdAsync(int id)
    {
        try
        {
            return await (from model in context.Model
                    where model.Status == 1 && model.Id == id
                    select mapper.Map<ModelDto>(model))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting model by id - GetModelByIdAsync");
            return null;
        }
    }

    public async Task<List<DropdownDto>?> GetModelsForDropdownAsync()
    {
        try
        {
            return await (from model in context.Model
                where model.Status == 1
                orderby model.Name
                select new DropdownDto
                {
                    Id = model.Id,
                    Name = model.Name
                }).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting models for dropdown - GetModelsForDropdownAsync");
            return null;
        }
    }
    
    public async Task<List<ModelDto>?> GetModelsByMakeIdAsync(int makeId)
    {
        try
        {
            return await (from model in context.Model
                join make in context.Make
                    on model.MakeId equals make.Id
                where (makeId == 0 || model.MakeId == makeId) && make.Status == 1
                orderby model.Name
                select new ModelDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status,
                    MakeId = make.Id,
                    MakeName = make.Name
                }).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting models by make - GetMakesByCategoryIdAsync");
            return null;
        }
    }

    public async Task<ModelDto?> CreateModelAsync(ModelDto modelDto)
    {
        try
        {
            var recordToCreate = mapper.Map<Model>(modelDto);
            var result = await context.Model.AddAsync(recordToCreate);
            await context.SaveChangesAsync();

            return mapper.Map<ModelDto>(result.Entity);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating model - CreateModelAsync");
            return null;
        }
    }

    public async Task<ModelDto?> UpdateModelAsync(ModelDto modelDto)
    {
        try
        {
            var response = await context.Model.FindAsync(modelDto.Id);

            if (response == null) return null;

            response.Id = modelDto.Id;
            response.Name = modelDto.Name;
            response.Description = modelDto.Description;
            response.Status = modelDto.Status;
            response.MakeId = modelDto.MakeId;

            await context.SaveChangesAsync();

            return mapper.Map<ModelDto>(response);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating model - UpdateModelAsync");
            return null;
        }
    }

    public async Task DeleteModelAsync(int id)
    {
        try
        {
            var response = await context.Model.Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (response != null)
            {
                context.Model.Remove(response);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting model - DeleteModelAsync");
        }
    }
}