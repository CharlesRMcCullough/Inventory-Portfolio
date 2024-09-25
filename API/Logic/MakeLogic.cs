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
        return await context.Make
            .Where(p => p.Status == 1)
            .Join(
                context.Category, // Inner sequence (the table to join with)
                make => make.CategoryId, // Outer key selector (Make's CategoryId)
                category => category.Id, // Inner key selector (Category's Id)
                (make, category) => make // Result selector (the result you want, in this case, the make)
            )
            .OrderBy(make => make.Name) // Order by the Make name
            .Select(make => new MakeDto
            {
                Id = make.Id,
                Name = make.Name,
                Description = make.Description,
                Status = make.Status,
                CategoryId = make.CategoryId,
                CategoryName = make.Category.Name
            }) // Map the Make entity to MakeDto
            .ToListAsync();
    }

    public async Task<List<MakeDto>> GetMakesByCategoryIdAsync(int categoryId)
    {
        return await (from make in context.Make
            join category in context.Category
                on make.CategoryId equals category.Id
            where (categoryId == 0 || make.CategoryId == categoryId) && make.Status == 1
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


        // return await context.Make
        //     .Where(p => p.CategoryId == categoryId && p.Status == 1)
        //     .Join(
        //         context.Category, // Inner sequence (the table to join with)
        //         make => make.CategoryId, // Outer key selector (Make's CategoryId)
        //         category => category.Id, // Inner key selector (Category's Id)
        //         (make, category) => make // Result selector (the result you want, in this case, the make)
        //     )
        //     .OrderBy(make => make.Name) // Order by the Make name
        //     .Select(make => new MakeDto
        //     {
        //         Id = make.Id,
        //         Name = make.Name,
        //         Description = make.Description,
        //         Status = make.Status,
        //         CategoryId = make.CategoryId,
        //         CategoryName = make.Category.Name
        //     }) // Map the Make entity to MakeDto
        //             .ToListAsync();
    }


    public async Task<MakeDto?> GetMakeByIdAsync(int id)
    {
        return await (from make in context.Make
            where (make.Id == id && make.Status == 1)
            orderby make.Name
            select new MakeDto
            {
                Id = make.Id,
                Name = make.Name,
                Description = make.Description,
                Status = make.Status,
                CategoryId = make.CategoryId
            }).FirstOrDefaultAsync() ?? new MakeDto();
    }

    public async Task<List<DropdownDto>> GetMakesForDropdownAsync()
    {
        return await (from make in context.Make
            where make.Status == 1
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

    public async Task<MakeDto?> UpdateMakeAsync(MakeDto makeDto)
    {
        var response = await context.Make.FindAsync(makeDto.Id);

        if (response == null) return null;

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