using Microsoft.AspNetCore.Mvc.Razor;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Application.Repository;
using UXComex_challenge.Application.Services;
using UXComex_challenge.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Web/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Web/Views/Shared/{0}.cshtml");
});

builder.Services.AddScoped<IRepository<Client>, ClientRepository>();
builder.Services.AddScoped<IService<Client>, ClientServices>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IService<Product>, ProductServices>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IService<Order>, OrderServices>();
builder.Services.AddScoped<IRepository<OrderNotification>, OrderNotificationRepository>();
builder.Services.AddScoped<IService<OrderNotification>, OrderNotificationServices>();

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/NotFound");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.Run();
