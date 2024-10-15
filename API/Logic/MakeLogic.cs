using API.Data;
using API.DTOs;
using API.Entities;
using API.Logic.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Exceptions;

namespace API.Logic;

public class MakeLogic(InventoryDbContext context, IMapper mapper) : IMakeLogic
{
    public async Task<List<MakeDto>> GetMakesAsync()
    {
        return await context.Make
            .Where(p => p.Status == true)
            .Join(
                context.Category, 
                make => make.CategoryId, 
                category => category.Id, 
                (make, category) => make 
            )
            .OrderBy(make => make.Name) 
            .Select(make => new MakeDto
            {
                Id = make.Id,
                Name = make.Name,
                Description = make.Description,
                Status = make.Status,
                CategoryId = make.CategoryId,
                CategoryName = make.Category.Name
            }) 
            .ToListAsync();
    }

    public async Task<List<MakeDto>> GetMakesByCategoryIdAsync(int categoryId)
    {
            return await (from make in context.Make
                join category in context.Category
                    on make.CategoryId equals category.Id
                where (categoryId == 0 || make.CategoryId == categoryId) && make.Status == true
                orderby make.Name
                select new MakeDto
                {
                    Id = make.Id,
                    Name = make.Name,
                    Description = make.Description,
                    Status = make.Status,
                    CategoryId = make.CategoryId,
                    CategoryName = category.Name
                }).ToListAsync();
    }

    public async Task<MakeDto> GetMakeByIdAsync(int makeId)
    {
            return await context.Make
                .Where(m => m.Id == makeId && m.Status == true)
                .Select(m => new MakeDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Status = m.Status,
                    CategoryId = m.CategoryId
                })
                .FirstOrDefaultAsync() ?? new MakeDto();
    }

    public async Task<List<DropdownDto>> GetMakesForDropdownAsync(int categoryId = 0)
    {
            return await (from make in context.Make
                where (categoryId == 0 || make.CategoryId == categoryId) && make.Status == true
                orderby make.Name
                select new DropdownDto
                {
                    Id = make.Id,
                    Name = make.Name
                }).ToListAsync();
    }

    public async Task<MakeDto> CreateMakeAsync(MakeDto makeDto)
    {
            var recordToCreate = mapper.Map<Make>(makeDto);
            var result = await context.Make.AddAsync(recordToCreate);
            await context.SaveChangesAsync();

            return mapper.Map<MakeDto>(result.Entity);
    }

    public async Task<MakeDto> UpdateMakeAsync(MakeDto makeDto)
    {
            var response = await context.Make.FindAsync(makeDto.Id);

            if (response == null)
            {
                throw new CustomExceptions.NotFoundException($"Make with id {makeDto.Id} was not found");
            }

            response.Id = makeDto.Id;
            response.Name = makeDto.Name;
            response.Description = makeDto.Description;
            response.Status = makeDto.Status;
            response.CategoryId = makeDto.CategoryId;

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