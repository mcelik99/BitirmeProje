using Microsoft.EntityFrameworkCore;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60 * 12);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<BitirmeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<BitirmeDBContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "BitirmeIdentityCookie";
    options.Cookie.HttpOnly = true;

    options.LoginPath = "/Login/Academician";
    options.LogoutPath = "/Login/Out";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.SlidingExpiration = true;
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    //options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapAreaControllerRoute(
            name: "Admin",
            areaName: "Admin",
            pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
            name: "Student",
            areaName: "Student",
            pattern: "Student/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Run();
