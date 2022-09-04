using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using WebUI.Models;
using Infrastructure;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public CartController(BeautyShopDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Корзина покупок";
            var userCart = new ShoppingCart();
            var cartVm = new CartVm();

            if(HttpContext.Request.Cookies.ContainsKey("Cart"))
            {
                var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
                var cart = _context.ShoppingCarts.Include(p => p.ProductInCarts).ThenInclude(p => p.Product).SingleOrDefault(p=>p.Id == guid);
                if(cart != null)
                    cartVm.ProductsInCarts = cart.ProductInCarts;
                if(cart == null)
                {
                    _context.ShoppingCarts.Add(new ShoppingCart { Id = guid });
                    _context.SaveChanges();
                }

            }
            else
            {
                var guid = Guid.NewGuid();
                _context.ShoppingCarts.Add(new ShoppingCart { Id = guid });
                _context.SaveChanges();
                
                HttpContext.Response.Cookies.Append("Cart", guid.ToString(), new CookieOptions { MaxAge = TimeSpan.FromDays(7)});

                userCart = _context.ShoppingCarts.Find(guid);
            }
            return View(cartVm);
        }
        [HttpPost]
        public IActionResult Add(int id, int amount)
        {
            var prod = _context.Products.Find(id);
            var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
            var cart = _context.ShoppingCarts.Find(guid);

            var prodInCart = _context.ProductInCarts.FirstOrDefault(p=>p.ProductId == id && p.ShoppingCartId == guid);
            if (prodInCart != null)
            {
                prodInCart.Amount += amount;
            }
            else if (cart != null)
            {
                cart.ProductInCarts.Add(new ProductInCart
                {
                    ProductId = prod!.ProductId,
                    ShoppingCartId = guid,
                    Amount = amount
                });
            }

            _context.SaveChanges();


            var s = HttpContext.Request.Headers.Referer.ToString();
            return Redirect(s);
        }
        public IActionResult Delete(int id)
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
            var prodInCart = _context.ProductInCarts.FirstOrDefault(p => p.Id == id && p.ShoppingCartId == guid);

            if(prodInCart != null)
            {
                _context.ProductInCarts.Remove(prodInCart);
                _context.SaveChanges();
            }

            var s = HttpContext.Request.Headers.Referer.ToString();
            return Redirect(s);
        }

        public IActionResult Edit(int id)
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
            var productInCart = _context.ProductInCarts.Where(p => p.Id == id).Include(p => p.Product);
            var entity = productInCart.FirstOrDefault(p=>p.Id == id);
            var editAmountVm = new CartEditAmountVm();

            if(entity != null) 
            {
                editAmountVm.Id = id; 
                editAmountVm.Name = entity.Product.Name;
                editAmountVm.Amount = entity.Amount;
            }

            return View(editAmountVm);
        }
        [HttpPost]
        public IActionResult EditAmount(int id, int amount)
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
            var cart = _context.ShoppingCarts.Find(guid);
            var prodInCart = _context.ProductInCarts.FirstOrDefault(p => p.Id == id && p.ShoppingCartId == guid);

            if(prodInCart != null)
                prodInCart.Amount = amount;

            _context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public IActionResult ClearCart()
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["Cart"]!);
            var cart = _context.ShoppingCarts.Find(guid);
            _context.ShoppingCarts.Remove(cart);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
        
    }
}
