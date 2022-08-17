using Domain.Entities;

namespace WebUI.Models
{
    public class CatalogPageViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginationViewModel Pvm { get; set; }
        public CatalogPageViewModel(IEnumerable<Product> products, PaginationViewModel pvm)
        {
            Products = products;
            Pvm = pvm;
        }
    }
}
