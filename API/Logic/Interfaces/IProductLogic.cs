using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Logic;

public interface IProductLogic
{
    Task<ActionResult<List<ProductDto>>> GetProductsAsync();
    Task<ActionResult<ProductDto>> GetProductByIdAsync(int id);
}