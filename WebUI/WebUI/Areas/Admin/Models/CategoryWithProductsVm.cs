using Domain.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class CategoryWithProductsVm
    {
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
