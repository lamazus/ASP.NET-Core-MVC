using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Admin.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public CategoryController(BeautyShopDbContext context)
        {
            _context = context;
        }
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
            var categoryVm = new CategoryWithProductsVm { Category = category, Products = products };

            ViewData["referer"] = HttpContext.Request.Headers.Referer;
            return View(categoryVm);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string name)
        {
            if (ModelState.IsValid)
            {

                var category = new Category { Name = name };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                return View();
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, string name)
        {
            if (ModelState.IsValid)
            {
                var category = await _context.Categories.FindAsync(id);
                category.Name = name;
                _context.Update(category);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Edit), new { categoryId = id });
            }
                return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, string confirm)
        {
            if(confirm =="Да")
            {
                var category = await _context.Categories.FindAsync(id);

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
