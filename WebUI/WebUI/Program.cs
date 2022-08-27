using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Domain.Interfaces;
using Infrastructure;
using WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false)
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/{0}.cshtml");
    });

builder.Services.AddDbContext<BeautyShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        b=>b.MigrationsAssembly("Infrastructure"));
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/manage/login";
    });

builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

var app = builder.Build();

app.UseStatusCodePages();
app.UseRouting();
app.UseStaticFiles();
app.UseMvc();

app.UseAuthentication();;
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "admin_area",
    areaName: "Admin",
    pattern: "manage/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
