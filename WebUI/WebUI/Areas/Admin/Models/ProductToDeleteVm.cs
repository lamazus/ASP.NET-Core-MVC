using Domain.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class ProductToDeleteVm
    {
        public int ProductId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
    }
}
