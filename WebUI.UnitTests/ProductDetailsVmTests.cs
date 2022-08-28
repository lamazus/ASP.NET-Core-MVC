using Xunit;
using WebUI.Models;
using Domain.Entities;
using System.Collections.Generic;

namespace WebUI.UnitTests
{
    public class ProductDetailsVmTests
    {
        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(5, 1, 2, 2)]
        [InlineData(3, 1, 5, 3)]
        public void CalculateRating_Return_AverageNumber(int a, int b, int c, int result)
        {
            var product = new ProductDetailsVm();
            product.Comments.AddRange(new List<Comment>
            {
                new Comment{ Rating = a },
                new Comment{ Rating = b },
                new Comment{ Rating = c }
            });

            var res = product.CalculateRating();

            Assert.Equal(result, res);
        }
    }
}
