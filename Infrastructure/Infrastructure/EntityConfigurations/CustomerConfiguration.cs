using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasMany(p => p.Orders)
                .WithOne(p => p.Customer);

            builder.HasKey(p => p.CustomerId);
            builder.Property(p => p.CustomerId).HasDefaultValueSql("NEWID()");

            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.TelephoneNumber).IsRequired().HasMaxLength(12);
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.City).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Address).IsRequired().HasMaxLength(100);
        }
    }
}