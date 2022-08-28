using Domain.Entities;

namespace WebUI.Models
{
    public class CartVm
    {
        public IEnumerable<ProductInCart>? ProductsInCarts { get; set; }
        public decimal ComputePrice()
        {
            decimal TotalPrice = 0;

            if(ProductsInCarts != null)
            {
                foreach(var p in ProductsInCarts)
                {
                    TotalPrice += p.Amount * p.Product.Price;
                }
            }
            return TotalPrice;
        }
    }
}
