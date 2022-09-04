using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace WebUI.Components
{
    public class DisplayRateStars : ViewComponent
    {
        public IViewComponentResult Invoke(Product product)
        {
            return View(product);
        }
    }
}
