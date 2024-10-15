using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Logic.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Logic;

public class ModelLogic(InventoryDbContext context, IMapper mapper) : IModelLogic
{
    public async Task<List<ModelDto>> GetModelsAsync()
    {
            return await (from model in context.Model
                join make in context.Make
                    on model.MakeId equals make.Id
                where model.Status == true
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

    public async Task<ModelDto?> GetModelByIdAsync(int id)
    {
            return await (from model in context.Model
                    where model.Status == true && model.Id == id
                    select mapper.Map<ModelDto>(model))
                .FirstOrDefaultAsync();
    }

    public async Task<List<DropdownDto>> GetModelsForDropdownAsync(int makeId = 0)
    {
            return await (from model in context.Model
                where (makeId == 0 || model.MakeId == makeId && model.Status == true)
                orderby model.Name
                select new DropdownDto
                {
                    Id = model.Id,
                    Name = model.Name
                }).ToListAsync();
    }

    public async Task<List<ModelDto>> GetModelsByMakeIdAsync(int makeId)
    {
            return await (from model in context.Model
                join make in context.Make
                    on model.MakeId equals make.Id
                where (makeId == 0 || model.MakeId == makeId) && make.Status == true
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

    public async Task<ModelDto?> CreateModelAsync(ModelDto modelDto)
    {
            var recordToCreate = mapper.Map<Model>(modelDto);
            var result = await context.Model.AddAsync(recordToCreate);
            await context.SaveChangesAsync();

            return mapper.Map<ModelDto>(result.Entity);
    }

    public async Task<ModelDto?> UpdateModelAsync(ModelDto modelDto)
    {
        var model = await context.Model.FindAsync(modelDto.Id);

        if (model == null)
        {
            throw new CustomExceptions.NotFoundException($"Model with id {modelDto.Id} was not found");
        }

        model.Name = modelDto.Name;
        model.Description = modelDto.Description;
        model.Status = modelDto.Status;
        model.MakeId = modelDto.MakeId;

        await context.SaveChangesAsync();

        return mapper.Map<ModelDto>(model);
    }

    public async Task DeleteModelAsync(int id)
    {
            var response = await context.Model.Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (response == null)
            {
                throw new CustomExceptions.NotFoundException($"Model with id {id} was not found");
            }
            
            context.Model.Remove(response);
            await context.SaveChangesAsync();
    }
}