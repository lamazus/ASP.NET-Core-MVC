using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public ProductController(BeautyShopDbContext context)
        {
            _context = context;
        }
        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            return View(products);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var product = await _context.Products.Include(p => p.Category).Include(p => p.Comments).FirstAsync(p => p.ProductId == id);

            ViewData["referer"] = HttpContext.Request.Headers.Referer;
            return View(product);
        }

        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection form)
        {
            var categoryId = Convert.ToInt32(form["categoryId"]);
            var name = form["name"];
            var price = Convert.ToDecimal(form["price"]);
            var stock = Convert.ToInt32(form["stock"]);
            var description = form["description"];
            var image = HttpContext.Request.Form.Files.First();

            var uploadPath = Path.Combine($"wwwroot/images/products/", image.FileName);
            using (FileStream fs = new FileStream(uploadPath, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }

            var newProd = new Product
            {
                CategoryId = categoryId,
                Name = name,
                Price = price,
                Stock = stock,
                Description = description,
                ImageName = image.FileName
            };
            _context.Products.Add(newProd);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
                return View(nameof(Index));

            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection form)
        {
            var categoryId = Convert.ToInt32(form["categoryId"]);
            var name = form["name"];
            var price = Convert.ToDecimal(form["price"]);
            var stock = Convert.ToInt32(form["stock"]); 
            var description = form["description"];



            var editedProd = await _context.Products.FindAsync(id);
            if (editedProd == null)
                return RedirectToAction(nameof(Index));

            editedProd.Name = name;
            editedProd.CategoryId = categoryId;
            editedProd.Price = price;
            editedProd.Stock = stock;
            editedProd.Description = description;
                
            var image = HttpContext.Request.Form.Files.GetFile("image");
            if (image != null)
            {
                var oldImage = Path.Combine("wwwroot/images/products/", editedProd.ImageName);
                System.IO.File.Delete(oldImage);

                var path = Path.Combine("wwwroot/images/products/", image.FileName);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(fs);
                }
                editedProd.ImageName = image.FileName;
            }

            _context.Products.Update(editedProd);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = id });
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var prod = await _context.Products.Include(p=>p.Category).FirstOrDefaultAsync(p=>p.ProductId == id);
            if (prod == null)
                return RedirectToAction(nameof(Index));

            var prodVm = new ProductToDeleteVm
            {
                Name = prod.Name,
                Category = prod.Category,
                ProductId = prod.ProductId,
                ImageName = prod.ImageName
            };

            return View(prodVm);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection form)
        {
            var confirm = form["confirm"];
            if(confirm == "Да")
            {
                var prod = await _context.Products.FindAsync(id);
                if (prod == null)
                    return RedirectToAction(nameof(Index));

                System.IO.File.Delete(Path.Combine("wwwroot/images/products", prod.ImageName));
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
