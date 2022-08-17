using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Orders);

            builder.Property(p => p.TotalPrice).IsRequired().HasColumnType("money");
            builder.Property(p => p.DateOfOrder).IsRequired();
            builder.Property(p => p.IsDelivered).HasDefaultValue(false);
        }
    }
}