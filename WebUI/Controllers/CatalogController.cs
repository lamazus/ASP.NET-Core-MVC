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
        public async Task<IActionResult> Index(int page = 1)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("Cart"))
            {
                var guid = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("Cart", guid.ToString(), new CookieOptions { MaxAge = TimeSpan.FromDays(7)});
                _context.ShoppingCarts.Add(new ShoppingCart { Id = guid });
                await _context.SaveChangesAsync();
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.MostPopularity = _context.Products.Include(p => p.Comments).OrderByDescending(p => p.NumberOfOrders).Where(p=>p.Stock>0).Take(4);

            return View();
        }
        // Фильтрация по категориям
        public async Task<IActionResult> Category(int id, int page = 1)
        {
            ViewBag.SelectedCategory = await _context.Categories.FirstAsync(p => p.CategoryId == id);
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.MinPrice = Convert.ToInt32(await _context.Products.MinAsync(p=>p.Price));
            ViewBag.MaxPrice = Convert.ToInt32(await _context.Products.MaxAsync(p=>p.Price));

            var productsFiltered = await _context.Products.Include(p=>p.Comments).Where(p => p.CategoryId == id).ToListAsync();

            AddPagination(productsFiltered, 10, page, out CatalogPageVm catalogPvm);

            return View(catalogPvm);
        }

        // Подробное описание товара
        public IActionResult Details(int id)
        {
            var products = _context.Products.Include(p=>p.Comments).Include(p=>p.Category).FirstOrDefault(p=>p.ProductId == id);
            var product = _context.Products.Find(id);
            var productVm = new ProductDetailsVm();

            if(product != null)
            {
                productVm.Id = product.ProductId;
                productVm.Name = product.Name;
                productVm.CategoryId = product.CategoryId;
                productVm.Category = product.Category;
                productVm.Comments = product.Comments;
                productVm.ImageName = product.ImageName;
                productVm.NumberOfOrder = product.NumberOfOrders;
                productVm.Price = product.Price;
                productVm.Description = product.Description;
                productVm.Stock = product.Stock;
            }

            return View(productVm);
        }

        // Сортировка товаров
         public async Task<IActionResult> Sort(int categoryId, string sortMethod, int page = 1)
         {
            ViewBag.SelectedCategory = await _context.Categories.FirstAsync(p => p.CategoryId == categoryId);
            ViewBag.Categories = await _context.Categories.ToListAsync();

             var products = await _context.Products.Include(p=>p.Comments).Where(p => p.CategoryId == categoryId).ToListAsync();

            SortProducts(ref products, sortMethod);
            AddPagination(products, 10, page, out CatalogPageVm catalogPvm);

            return View("Category", catalogPvm);
         }

        // Поиск по слову
        public IActionResult Search(string searchString, string sortMethod, int page = 1)
        {
            var products = _context.Products.Include(p => p.Comments).Where(p => p.Name.ToLower().Contains(searchString.Trim().ToLower())).ToList();
            ViewBag.SearchString = searchString;
            ViewBag.ResultCount = products.Count;
            ViewBag.SortMethod = sortMethod;

            SortProducts(ref products, sortMethod);
            AddPagination(products, 10, page, out CatalogPageVm catalogPvm);

            return View(catalogPvm);
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

           var products = await _context.Products.Include(p => p.Comments).Where(p=>p.Price >= _minPrice && p.Price <= _maxPrice && p.CategoryId==categoryId).ToListAsync();

            if (availability == "on")
            {
                products = products.Where(p => p.Stock > 0).ToList();
                ViewBag.Availability = availability;
            }

            SortProducts(ref products, sortMethod);
            AddPagination(products, 10, page, out CatalogPageVm catalogPvm);

            return View(catalogPvm);
        }

        [NonAction]
        public static void SortProducts(ref List<Product> products, string sortMethod) 
        {
            switch (sortMethod)
            {
                case "name":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "name-desc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                case "price":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price-desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "popularity":
                    products = products.OrderBy(p => p.NumberOfOrders).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
            }
        }

        [NonAction]
        public static void AddPagination(List<Product> products, int pageSize, int page, out CatalogPageVm catalogPvm)
        {
            var pvm = new PaginationVm(products.Count, page, pageSize);
            var productsPage = products.Skip(pageSize * (pvm.CurrentPage - 1)).Take(pageSize);
            catalogPvm = new CatalogPageVm(productsPage, pvm);
        }
    }
}
