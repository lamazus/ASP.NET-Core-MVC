using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult HowToBuy()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }


    }
}
