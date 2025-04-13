using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Database Context with SQLite
builder.Services.AddDbContext<EventAppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Identity (if using authentication)
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<EventAppDbContext>();


// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login"; // Redirect to login page
        options.AccessDeniedPath = "/Users/AccessDenied";
    });



builder.Services.AddAuthorization();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Pages"); // Require authentication for all pages
    options.Conventions.AllowAnonymousToPage("/Users/Login"); // Allow login page without authentication
    options.Conventions.AllowAnonymousToPage("/Users/Register"); // Allow register page without authentication
    options.Conventions.AllowAnonymousToPage("/Admins/AdminLogIn"); // Allow register page without authentication
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var assetsPath = Path.Combine(builder.Environment.ContentRootPath, "assets");

if (Directory.Exists(assetsPath)) // ✅ Check if assets folder exists
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(assetsPath),
        RequestPath = "/assets"
    });
}


// Seed admin accounts if they don't already exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventAppDbContext>();
    var passwordHasher = new PasswordHasher<IdentityUser>();

    // Check if admins already exist
    if (!context.Admins.Any())
    {
        var admin1 = new Admin
        {
            Email = "admin1@urnettribe.com",
            Password = passwordHasher.HashPassword(new IdentityUser(), "admin1password"),  // Hash password
            AdminKey = passwordHasher.HashPassword(new IdentityUser(), "superAdminUser"),  // Hash AdminKey
            CreatedAt = DateTime.UtcNow,
            SessionToken = null
        };

        var admin2 = new Admin
        {
            Email = "aadmin2@urnettribe.comm",
            Password = passwordHasher.HashPassword(new IdentityUser(), "admin2password"),
            AdminKey = passwordHasher.HashPassword(new IdentityUser(), "superUserAdmin"),
            CreatedAt = DateTime.UtcNow,
            SessionToken = null
        };

        var admin3 = new Admin
        {
            Email = "admin3@urnettribe.com",
            Password = passwordHasher.HashPassword(new IdentityUser(), "admin3password"),
            AdminKey = passwordHasher.HashPassword(new IdentityUser(), "adminSuperUser"),
            CreatedAt = DateTime.UtcNow,
            SessionToken = null
        };

        context.Admins.AddRange(admin1, admin2, admin3);
        context.SaveChanges();
    }
}








app.UseRouting();

app.UseAuthorization();

app.UseSession(); 

app.MapRazorPages();
app.UseStaticFiles();

app.Run();
