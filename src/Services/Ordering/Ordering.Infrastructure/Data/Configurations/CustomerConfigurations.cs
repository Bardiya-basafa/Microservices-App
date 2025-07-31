namespace Ordering.Infrastructure.Data;

using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class CustomerConfigurations : IEntityTypeConfiguration<Customer> {

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(customerId => customerId.Value, dbId => CustomerId.Of(dbId));

        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(c => c.Email).IsUnique();
    }

}
