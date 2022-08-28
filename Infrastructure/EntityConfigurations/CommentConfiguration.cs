using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(p => p.Product)
                .WithMany(p => p.Comments);

            builder.HasKey(p => p.CommentId);
            builder.Property(p=>p.CommentId).HasDefaultValueSql("NEWID()");

            builder.Property(p => p.Username).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Rating).IsRequired();
            builder.Property(p => p.Text).IsRequired().HasMaxLength(255);
        }
    }
}