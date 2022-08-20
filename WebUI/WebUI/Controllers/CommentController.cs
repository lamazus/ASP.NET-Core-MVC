using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class CommentController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public CommentController(BeautyShopDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string name, int rating, string text)
        {
            var prod = await _context.Products.FindAsync(id);

            if (prod == null)
                return NotFound();

            _context.Comments.Add(new Comment
            {
                ProductId = id,
                Username = name.Trim(),
                Text = text,
                Rating = rating,
                CreatedAt = DateTime.Now
            });

            _context.SaveChanges();

            var referer = HttpContext.Request.Headers.Referer;

            return Redirect(referer);

        }
    }
}
