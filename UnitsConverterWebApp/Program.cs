using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using UnitsConverterWebApp.Data;
using UnitsConverterWebApp.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UnitsConverterWebAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UnitsConverterWebAppContext") ?? throw new InvalidOperationException("Connection string 'UnitsConverterWebAppContext' not found.")));

//for accounts
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

 

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//setup account 

app.UseSession();



app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Converter}/{action=Index}/{id?}");

app.Run();
