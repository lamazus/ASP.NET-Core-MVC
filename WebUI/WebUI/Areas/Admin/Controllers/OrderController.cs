using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public OrderController(BeautyShopDbContext context)
        {
            _context = context;
        }
        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            var orders = await _context.Orders.Include(p => p.Products).Include(p => p.Customer).ToListAsync();

            return View(orders);
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int orderId)
        {
            var order = await _context.Orders.Include(p => p.Products).ThenInclude(p=>p.Product).Include(p => p.Customer).FirstOrDefaultAsync(p => p.OrderId == orderId);

            var orderVm = new OrderDetailVm
            {
                OrderId = order.OrderId,
                Customer = order.Customer,
                DateOfOrder = order.DateOfOrder,
                DeliveryDate = order.DeliveryDate,
                DeliveryTime = order.DeliveryTime,
                Products = order.Products,
                TotalPrice = order.TotalPrice,
                IsDelivered = order.IsDelivered,
                IsPaid = order.IsPaid,
                Commentary = order.Commentary
            };

            ViewData["referer"] = HttpContext.Request.Headers.Referer;
            return View(orderVm);
        }

        // GET: OrderController/Edit/5
        public async Task<ActionResult> Edit(int orderId)
        {
            var order = await _context.Orders.Include(p=>p.Customer).Include(p=>p.Products).FirstOrDefaultAsync(p=>p.OrderId == orderId);

            var orderVm = new OrderDetailVm
            {
                OrderId = order.OrderId,
                Customer = order.Customer,
                DateOfOrder = order.DateOfOrder,
                DeliveryDate = order.DeliveryDate,
                DeliveryTime = order.DeliveryTime,
                Products = order.Products,
                TotalPrice = order.TotalPrice,
                IsDelivered = order.IsDelivered,
                IsPaid = order.IsPaid,
                Commentary = order.Commentary
            };

            return View(orderVm);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int orderId, IFormCollection form)
        {
            var order = await _context.Orders.FindAsync(orderId);
            order.DeliveryDate = DateTime.Parse(form["DeliveryDate"]);
            order.DeliveryTime = form["DeliveryTime"];
            order.Commentary = form["Commentary"];

            return RedirectToAction("Details", new { orderId = orderId });
        }
    }
}
