using API.DTOs;

namespace API.Logic.Interfaces;

public interface IItemLogic
{
    Task<IReadOnlyList<ItemDto>> GetItemsAsync();
    Task<IReadOnlyList<ItemDto>> GetItemsByProductIdAsync(int id);
    Task<ItemDto?> GetItemByIdAsync(int id);
}