using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Infrastructure;
using WebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public OrderController(BeautyShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> Payment(int orderId = 0, bool isPaid = false)
        {
            ViewBag.OrderId = orderId;
            if (isPaid && orderId != 0)
            {
                var order = await _context.Orders.FindAsync(orderId);
                order.IsPaid = true;
                _context.Orders.Update(order);
                _context.SaveChanges();

                HttpContext.Response.Cookies.Delete("Cart");
                return RedirectToAction("Success");
            }

            return View();
        }
        public async Task<IActionResult> AddOrder(string name, string telephoneNumber, string email, string city,
            string address, string commentary, DateTime deliveryDate, string deliveryTime)
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
            var cart = await _context.ShoppingCarts.Include(p=>p.ProductInCarts).ThenInclude(p=>p.Product).FirstOrDefaultAsync(p=>p.Id == guid);

            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                Name = name,
                Email = email,
                TelephoneNumber = telephoneNumber,
                City = city,
                Address = address
            };

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                Commentary = commentary,
                DateOfOrder = DateTime.Now,
                Products = cart.ProductInCarts,
                TotalPrice = ComputePrice(cart.ProductInCarts),
                DeliveryDate = deliveryDate,
                DeliveryTime = deliveryTime

            };
             _context.Customers.Add(customer);
            _context.SaveChanges();

            _context.Orders.Add(order);
            _context.SaveChanges();


            foreach (var prod in cart.ProductInCarts)
            {
                var productInDb = await _context.Products.FindAsync(prod.ProductId);

                productInDb.NumberOfOrders += prod.Amount;
                if(productInDb.Stock >= prod.Amount)
                    productInDb.Stock -= prod.Amount;
                _context.Products.Update(productInDb);
                _context.SaveChanges();

            }

            return RedirectToAction("Payment", new { orderId = order.OrderId});

        }

        [NonAction]
        public decimal ComputePrice(IEnumerable<ProductInCart> products)
        {
            decimal TotalPrice = 0;

            if (products != null)
            {
                foreach (var p in products)
                {
                    TotalPrice += p.Amount * p.Product.Price;
                }
            }
            return TotalPrice;
        }
    }
}
