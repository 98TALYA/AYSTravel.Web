using AYSTravel.Data;
using AYSTravel.Logic;
using AYSTravel.Web.Data;
using AYSTravel.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Database
// ==========================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ==========================
// Identity
// ==========================
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ? Enregistrement du EmailSender (corrige ton erreur)
builder.Services.AddTransient<IEmailSender, FakeEmailSender>();

// ==========================
// MVC + Razor
// ==========================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ==========================
// Managers
// ==========================
builder.Services.AddScoped<MatchManager>();
builder.Services.AddScoped<ActiviteManager>();
builder.Services.AddScoped<MonumentManager>();
builder.Services.AddScoped<MoyenTransportManager>();
builder.Services.AddScoped<VilleManager>();
builder.Services.AddScoped<RestaurationManager>() ;
builder.Services.AddScoped<HotelManager>();

var app = builder.Build();

// ==========================
// Pipeline
// ==========================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages().WithStaticAssets();

// ==========================
// Seed Roles
// ==========================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();