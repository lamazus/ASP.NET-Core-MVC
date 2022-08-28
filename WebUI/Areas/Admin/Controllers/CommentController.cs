using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly BeautyShopDbContext _context;
        public CommentController(BeautyShopDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Delete(string id)
        {
            var comment = await _context.Comments.FindAsync(Guid.Parse(id));
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            var referer = HttpContext.Request.Headers.Referer;

            return Redirect(referer);
        }
    }
}
