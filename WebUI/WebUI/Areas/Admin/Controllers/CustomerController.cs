using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public CustomerController(BeautyShopDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.Include(p=>p.Orders).ToListAsync();

            return View(customers);
        }
        public async Task<IActionResult> Details(string customerId)
        {
            var customer = await _context.Customers.Include(p=>p.Orders).FirstOrDefaultAsync(p => p.CustomerId == Guid.Parse(customerId));

            var customerVm = new CustomerVm
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                TelephoneNumber = customer.TelephoneNumber,
                City = customer.City,
                Address = customer.Address,
                Orders = customer.Orders

            };

            ViewData["referer"] = HttpContext.Request.Headers.Referer;
            return View(customerVm);
        }
        public async Task<IActionResult> Edit(string customerId)
        {
            var customer = await _context.Customers.FindAsync(Guid.Parse(customerId));

            var customerVm = new CustomerVm
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                TelephoneNumber = customer.TelephoneNumber,
                Address = customer.Address,
                City = customer.City,
                Orders = customer.Orders
            };

            return View(customerVm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string customerId, IFormCollection form)
        {
            var customer = await _context.Customers.FindAsync(Guid.Parse(customerId));

            customer.Name = form["Name"];
            customer.Email = form["Email"];
            customer.TelephoneNumber = form["TelephoneNumber"];
            customer.City = form["City"];
            customer.Address = form["Address"];

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { customerId = customerId });

        }
    }
}
