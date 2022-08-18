using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(p => p.Comments)
                .WithOne(p => p.Product);


            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Price).IsRequired().HasColumnType("money");
            builder.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
            builder.Property(p => p.NumberOfOrders).IsRequired().HasDefaultValue(0);
            builder.Property(p => p.ImageName).HasDefaultValue("null").HasMaxLength(40);
            builder.Property(p => p.Description).HasMaxLength(255);
        }
    }
}