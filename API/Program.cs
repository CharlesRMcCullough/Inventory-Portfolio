using API.Data;
using API.Logic;
using API.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<InventoryDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductLogic, ProductLogic>();
builder.Services.AddScoped<ICategoryLogic, CategoryLogic>();
builder.Services.AddScoped<IMakeLogic, MakeLogic>();
builder.Services.AddScoped<IModelLogic, ModelLogic>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();