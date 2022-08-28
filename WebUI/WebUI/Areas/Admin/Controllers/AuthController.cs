using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Domain.Interfaces;
using Infrastructure;
using Domain.Entities;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("manage/login")]
    public class AuthController : Controller
    {
        private readonly IAuthenticateService _authService;
        private readonly BeautyShopDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(BeautyShopDbContext context, IAuthenticateService authService, IConfiguration config)
        {
            _context = context;
            _authService = authService;
            _config = config;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection form)
        {
            var username = form["login"].ToString();
            var password = form["password"].ToString();
            
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Username == username);

            if(user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (!_authService.VerifyPassword(password, user.PasswordHash))
                return View(nameof(Login));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(7) });

            return RedirectToAction("Index", "Home");

        }
    }
}
