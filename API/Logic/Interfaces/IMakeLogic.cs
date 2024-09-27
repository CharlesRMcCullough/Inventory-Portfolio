using API.DTOs;

namespace API.Logic.Interfaces;

public interface IMakeLogic
{
    Task<List<MakeDto>?> GetMakesAsync();
    Task<MakeDto?> GetMakeByIdAsync(int id);
    Task<List<MakeDto>?> GetMakesByCategoryIdAsync(int categoryId);
    Task<List<DropdownDto>?> GetMakesForDropdownAsync();
    Task<MakeDto?> CreateMakeAsync(MakeDto makeDto);
    Task<MakeDto?> UpdateMakeAsync(MakeDto makeDto);
    Task DeleteMakeAsync(int id);
}