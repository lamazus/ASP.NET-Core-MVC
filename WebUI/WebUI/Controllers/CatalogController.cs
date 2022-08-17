using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using WebUI.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Controllers
{
    [Controller]
    public class CatalogController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public CatalogController(BeautyShopDbContext context)
        {
            _context = context;
        }

        // Главная страница каталога
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("Cart"))
            {
                var guid = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("Cart", guid.ToString(), new CookieOptions { MaxAge = TimeSpan.FromDays(7)});
                _context.ShoppingCarts.Add(new Domain.Entities.ShoppingCart { Id = guid });
                await _context.SaveChangesAsync();
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View();
        }
        // Фильтрация по категориям
        public async Task<IActionResult> Category(int id, int page = 1)
        {
            ViewBag.SelectedCategory = await _context.Categories.FirstAsync(p => p.CategoryId == id);
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.MinPrice = Convert.ToInt32(await _context.Products.MinAsync(p=>p.Price));
            ViewBag.MaxPrice = Convert.ToInt32(await _context.Products.MaxAsync(p=>p.Price));

            var pageSize = 8;
            var productsFiltered = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();

            var pvm = new PaginationViewModel(productsFiltered.Count, page, pageSize);
            var productsPage = productsFiltered.Skip(pageSize * (pvm.CurrentPage - 1)).Take(pageSize);
            var catalogPvm = new CatalogPageViewModel(productsPage, pvm);

            return View(catalogPvm);
        }

        // Поиск по слову
        public IActionResult Search(string searchString, int page = 1, string sortMethod = "name")
        {
            int pageSize = 8;
            var result = _context.Products.Where(p => p.Name.ToLower().Contains(searchString.Trim().ToLower())).ToList();
            ViewBag.SearchString = searchString;
            ViewBag.ResultCount = result.Count;
            ViewBag.SortMethod = sortMethod;

            var sortedProducts = new List<Product>();
            switch (sortMethod)
            {
                case "name":
                    sortedProducts = result.OrderBy(p => p.Name).ToList();
                    break;
                case "name-desc":
                    sortedProducts = result.OrderByDescending(p => p.Name).ToList();
                    break;
                case "price":
                    sortedProducts = result.OrderBy(p => p.Price).ToList();
                    break;
                case "price-desc":
                    sortedProducts = result.OrderByDescending(p => p.Price).ToList();
                    break;
            }

            var pvm = new PaginationViewModel(sortedProducts.Count, page, pageSize);
            var productsPage = sortedProducts.Skip(pageSize * (pvm.CurrentPage - 1)).Take(pageSize);
            var catalogPvm = new CatalogPageViewModel(productsPage, pvm);
            return View(catalogPvm);
        }

        // Подробное описание товара
        public IActionResult Details(int id)
        {
            var products = _context.Products.Include(p=>p.Comments).Include(p=>p.Category).FirstOrDefault(p=>p.ProductId == id);
            var product = _context.Products.Find(id);
            var productVm = new CatalogProductDetailsViewModel();

            if(product != null)
            {
                productVm.Id = product.ProductId;
                productVm.Name = product.Name;
                productVm.CategoryId = product.CategoryId;
                productVm.Category = product.Category;
                productVm.Comments = product.Comments;
                productVm.ImageName = product.ImageName;
                productVm.NumberOfPurchase = product.NumberOfPurchases;
                productVm.Price = product.Price;
                productVm.Description = product.Description;
                productVm.Stock = product.Stock;
            }

            return View(productVm);
        }

        // Сортировка товаров
        public async Task<IActionResult> Sort(IEnumerable<Product> model, int categoryId, string sortMethod, int page = 1)
        {
            ViewBag.SelectedCategory = await _context.Categories.FirstAsync(p => p.CategoryId == categoryId);
            ViewBag.Categories = await _context.Categories.ToListAsync();

            var pageSize = 8;

            var sortedProducts = new List<Product>();
            switch (sortMethod)
            {
                case "name":
                    sortedProducts = model.OrderBy(p => p.Name).ToList();
                    break;
                case "name-desc":
                    sortedProducts = model.OrderByDescending(p => p.Name).ToList();
                    break;
                case "price":
                    sortedProducts = model.OrderBy(p => p.Price).ToList();
                    break;
                case "price-desc":
                    sortedProducts = model.OrderByDescending(p => p.Price).ToList();
                    break;
            }
            var pvm = new PaginationViewModel(sortedProducts.Count, page, pageSize);
            var productsPage = sortedProducts.Skip(pageSize * (pvm.CurrentPage - 1)).Take(pageSize);
            var catalogPvm = new CatalogPageViewModel(productsPage, pvm);

            return View("Category", catalogPvm);
        }
         public async Task<IActionResult> Sort(int categoryId, string sortMethod, int page = 1)
         {
            ViewBag.SelectedCategory = await _context.Categories.FirstAsync(p => p.CategoryId == categoryId);
            ViewBag.Categories = await _context.Categories.ToListAsync();

             var pageSize = 8;

             var productsFiltered = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
             var sortedProducts = new List<Product>();
             switch (sortMethod) 
             {
                 case "name":  sortedProducts = productsFiltered.OrderBy(p => p.Name).ToList();
                     break;
                 case "name-desc":  sortedProducts = productsFiltered.OrderByDescending(p => p.Name).ToList();
                     break;
                 case "price": sortedProducts = productsFiltered.OrderBy(p => p.Price).ToList();
                     break;
                 case "price-desc": sortedProducts = productsFiltered.OrderByDescending(p => p.Price).ToList();
                     break;
             }
             var pvm = new PaginationViewModel(sortedProducts.Count, page, pageSize);
             var productsPage = sortedProducts.Skip(pageSize * (pvm.CurrentPage - 1)).Take(pageSize);
             var catalogPvm = new CatalogPageViewModel(productsPage, pvm);

             return View("Category", catalogPvm);
         }

        // Отфильтровать выборку
        public async Task<IActionResult> Filter (int categoryId, int minPrice, int maxPrice, string availability, string sortMethod, int page = 1)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategory = await _context.Categories.FirstAsync(p => p.CategoryId == categoryId);
            ViewBag.Availability = availability;
            ViewBag.SortMethod = sortMethod;

            int _minPrice = 0;
            int _maxPrice = 0;
            if (minPrice == 0)
                _minPrice = Convert.ToInt32(await _context.Products.MinAsync(p => p.Price));
            else
                _minPrice = minPrice;
            if(maxPrice == 0 )
                _maxPrice = Convert.ToInt32(await _context.Products.MaxAsync(p => p.Price));
            else
                _maxPrice = maxPrice;

            ViewBag.MinPrice = _minPrice;
            ViewBag.MaxPrice = _maxPrice;

           var filtered = await _context.Products.Where(p=>p.Price >= _minPrice && p.Price <= _maxPrice && p.CategoryId==categoryId).ToListAsync();

            if (availability == "on")
            {
                filtered = filtered.Where(p => p.Stock > 0).ToList();
                ViewBag.Availability = availability;
            }

            var sortedProducts = new List<Product>();

            switch (sortMethod)
            {
                case "name":
                    sortedProducts = filtered.OrderBy(p => p.Name).ToList();
                    break;
                case "name-desc":
                    sortedProducts = filtered.OrderByDescending(p => p.Name).ToList();
                    break;
                case "price":
                    sortedProducts = filtered.OrderBy(p => p.Price).ToList();
                    break;
                case "price-desc":
                    sortedProducts = filtered.OrderByDescending(p => p.Price).ToList();
                    break;
                default: sortedProducts = filtered.OrderBy(p => p.Name).ToList();
                    break;
            }

            int pageSize = 8;
            var pvm = new PaginationViewModel(sortedProducts.Count, page, pageSize);
            var productsPage = sortedProducts.Skip(pageSize * (pvm.CurrentPage - 1)).Take(pageSize);
            var catalogPvm = new CatalogPageViewModel(productsPage, pvm);

            return View(catalogPvm);
        }

    }
}
