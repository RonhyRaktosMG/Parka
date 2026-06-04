using ListeEtudiant.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Repository;
using ParkaApp.Repository.Interfaces;
using ParkaApp.Services;
using ParkaApp.Services.Interfaces;

using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Base de donnée
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data source=parka.db"));

// Repository
builder.Services.AddTransient<IAreaRepository, AreaRepository>();
builder.Services.AddTransient<IPlaceRepository, PlaceRepository>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IOccupationRepository, OccupationRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();


// Services
builder.Services.AddTransient<IMVolaService, MVolaService>();
builder.Services.AddHttpClient<IMVolaService, MVolaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();


var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".mp4"] = "video/mp4";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
