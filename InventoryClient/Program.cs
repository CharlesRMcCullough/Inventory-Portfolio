using MudBlazor.Services;
using InventoryClient.Components;
using InventoryClient.Integrations;
using InventoryClient.Integrations.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ICategoryIntegration, CategoryIntegration>();
builder.Services.AddScoped<IMakeIntegration, MakeIntegration>();
builder.Services.AddScoped<IModelIntegration, ModelIntegration>();
builder.Services.AddScoped<IProductIntegration, ProductIntegration>();
builder.Services.AddScoped<IItemIntegration, ItemIntegration>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();