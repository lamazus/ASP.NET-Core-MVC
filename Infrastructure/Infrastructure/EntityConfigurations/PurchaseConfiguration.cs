using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Purchases);

            builder.Property(p => p.TotalPrice).IsRequired().HasColumnType("money");
            builder.Property(p => p.DateOfPurchase).IsRequired();
            builder.Property(p => p.IsDelivered).HasDefaultValue(false);
        }
    }
}