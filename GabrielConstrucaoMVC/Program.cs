using Microsoft.AspNetCore.Identity;
using GabrielConstrucaoMVC.Data;
using Microsoft.EntityFrameworkCore;
using GabrielConstrucaoMVC.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var culturaBrasileira = new CultureInfo("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = culturaBrasileira;
CultureInfo.DefaultThreadCurrentUICulture = culturaBrasileira;
builder.Services.AddSession();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Admins.Any())
    {
        var senhaInicial = app.Configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(senhaInicial))
        {
            throw new InvalidOperationException(
                "A senha inicial do administrador não foi configurada."
            );
        }

        var admin = new Admin
        {
            Usuario = "admin"
        };

        var passwordHasher = new PasswordHasher<Admin>();

        admin.Senha = passwordHasher.HashPassword(
            admin,
            senhaInicial
        );

        context.Admins.Add(admin);
        context.SaveChanges();
    }
}

app.Run();
