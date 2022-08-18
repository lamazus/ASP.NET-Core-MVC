using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations 
{
    public class ProductInCartConfiguration : IEntityTypeConfiguration<ProductInCart>
    {
        public void Configure(EntityTypeBuilder<ProductInCart> builder)
        {
            builder.HasOne(p => p.ShoppingCart)
                .WithMany(p => p.ProductInCarts).OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.ProductId).IsRequired();
        }

    }
}
