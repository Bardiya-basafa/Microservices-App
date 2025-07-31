namespace Ordering.Infrastructure.Data;

using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class OrderConfigurations : IEntityTypeConfiguration<Order> {

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithOne()
            .HasForeignKey<Order>(o => o.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ComplexProperty(o => o.OrderName,
        nameBuilder => {
            nameBuilder.Property(n => n.Value)
                .HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.ComplexProperty(o => o.ShippingAddress,
        addressBuilder => {
            addressBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(500)
                .IsRequired();

            addressBuilder.Property(a => a.City)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.State)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(120)
                .IsRequired();
        });

        builder.ComplexProperty(o => o.BillingAddress,
        addressBuilder => {
            addressBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(500)
                .IsRequired();

            addressBuilder.Property(a => a.City)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.State)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(120)
                .IsRequired();
        });

        builder.ComplexProperty(o => o.Payment,
        paymentBuilder => {
            paymentBuilder.Property(p => p.PaymentMethod);

            paymentBuilder.Property(p => p.CardName)
                .HasMaxLength(50)
                .IsRequired();

            paymentBuilder.Property(p => p.CardNumber)
                .HasMaxLength(50)
                .IsRequired();

            paymentBuilder.Property(p => p.CvvCode)
                .HasMaxLength(50)
                .IsRequired();

            paymentBuilder.Property(p => p.ExpirationDate)
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
            s => s.ToString(),
            dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);
    }

}
