using ListeEtudiant.Data;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Repository;
using ParkaApp.Repository.Interfaces;

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
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Area}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
