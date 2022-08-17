using Microsoft.EntityFrameworkCore;
using Infrastructure;


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


var app = builder.Build();

app.UseStatusCodePages();
app.UseRouting();
app.UseStaticFiles();
app.UseMvc();
app.UseAuthentication();;

app.MapControllerRoute(
    name: "catalog",
    pattern: "{controller=Catalog}/{action}/{id?}");

app.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
