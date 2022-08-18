using Microsoft.AspNetCore.Mvc;
using Infrastructure;

namespace WebUI.Controllers
{
    public class TrackingController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public TrackingController(BeautyShopDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            return View(order);
        }
    }
}
