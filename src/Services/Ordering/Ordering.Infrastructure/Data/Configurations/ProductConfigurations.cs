namespace Ordering.Infrastructure.Data;

using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class ProductConfigurations : IEntityTypeConfiguration<Product> {

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(productId => productId.Value, dbId => ProductId.Of(dbId));
    }

}
