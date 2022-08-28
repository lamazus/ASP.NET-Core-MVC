using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Components
{
    public class ProductCards : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<Product> products)
        {
            return View(products);
        }
    }
}
