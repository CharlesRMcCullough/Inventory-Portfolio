using API.Data;
using API.Entities;
using API.Logic;
using AutoMapper;

namespace API.UnitTests.CategoryTests;

public class CategoryTests
{
    
    [Fact]
    public void CategoryNameHasValue_NotNull_True()
    {
        var category = new Category { Id = 10, Name = "TestName" };
        
        var result = category.CategoryHasName();
        
        Assert.True(result);
        Assert.Contains("TestName", category.Name);
    }
}