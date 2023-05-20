using Microsoft.EntityFrameworkCore;
using BitirmeProjesi.Data;
using BitirmeProjesi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BitirmeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User,Role>().AddEntityFrameworkStores<BitirmeDBContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "BitirmeIdentityCookie";
    options.Cookie.HttpOnly = true;

    options.LoginPath = "/Sign/In";
    options.LogoutPath = "/Sign/Out";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.SlidingExpiration = true;
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    //options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapAreaControllerRoute(
            name: "Admin",
            areaName: "Admin",
            pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
