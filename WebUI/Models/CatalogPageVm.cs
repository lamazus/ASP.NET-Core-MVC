using Domain.Entities;

namespace WebUI.Models
{
    public class CatalogPageVm
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginationVm Pvm { get; set; }
        public CatalogPageVm(IEnumerable<Product> products, PaginationVm pvm)
        {
            Products = products;
            Pvm = pvm;
        }
    }
}
