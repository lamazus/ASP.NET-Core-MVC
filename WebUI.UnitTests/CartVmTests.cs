using Xunit;
using WebUI.Models;
using System.Collections.Generic;
using Domain.Entities;

namespace WebUI.UnitTests
{
    public class CartVmTests
    {
        [Fact]
        public void ComputePrice_Return_CorrectValue()
        {
            var cartVm = new CartVm();
            cartVm.ProductsInCarts = new List<ProductInCart>
            {
                new ProductInCart
                {
                    Product = new Product{ Price = 100 },
                    Amount = 10
                },
                new ProductInCart
                {
                    Product = new Product{ Price = 200 },
                    Amount = 1
                }
            };

            var result = cartVm.ComputePrice();

            Assert.Equal(1200, result);
        }
    }
}
