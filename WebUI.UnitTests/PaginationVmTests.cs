using WebUI.Models;
using Xunit;
using System.Collections.Generic;
using Domain.Entities;
using Moq;
using System.Linq;

namespace WebUI.UnitTests
{
    public class PaginationVmTests
    {
        [Fact]
        public void Pagination_Should_SplitCollectionIntoPages()
        {
            var products = new List<Product>
            {
                new Product{ProductId = 1},
                new Product{ProductId = 2},
                new Product{ProductId = 3},
                new Product{ProductId = 4},
                new Product{ProductId = 5},
                new Product{ProductId = 6},
                new Product{ProductId = 7},
                new Product{ProductId = 8}
            };

            var paginationVm = new PaginationVm(products.Count, 1, 3);
            var page1 = products.Skip(3 * (paginationVm.CurrentPage - 1)).Take(3);
            

            paginationVm = new PaginationVm(products.Count, 2, 3);
            var page2 = products.Skip(3 * (paginationVm.CurrentPage - 1)).Take(3);


            paginationVm = new PaginationVm(products.Count, 3, 3);
            var page3 = products.Skip(3 * (paginationVm.CurrentPage - 1)).Take(3);


            Assert.NotEmpty(page1);
            Assert.NotEmpty(page2);
            Assert.NotEmpty(page3);
            Assert.Equal(3, page1.Count());
            Assert.Equal(3, page2.Count());
            Assert.Equal(2, page3.Count());
        }
    }
}
