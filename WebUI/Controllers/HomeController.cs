using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Domain.Entities;

namespace WebUI.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public HomeController(BeautyShopDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("Cart"))
            {
                var guid = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("Cart", guid.ToString(), new CookieOptions { MaxAge = TimeSpan.FromDays(7) });
                _context.ShoppingCarts.Add(new ShoppingCart { Id = guid });
                await _context.SaveChangesAsync();
            }
            

            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }


    }
}
