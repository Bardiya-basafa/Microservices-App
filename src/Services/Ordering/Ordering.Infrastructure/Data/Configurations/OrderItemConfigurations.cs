namespace Ordering.Infrastructure.Data;

using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem> {

    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasConversion(oiId => oiId.Value, dbId => OrderItemId.Of(dbId));

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(oi => oi.Price).IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired();
    }

}
