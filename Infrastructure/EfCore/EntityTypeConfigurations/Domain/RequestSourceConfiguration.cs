using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Infrastructure.EfCore.EntityTypeConfigurations.Domain;

internal sealed class RequestSourceConfiguration : IEntityTypeConfiguration<RequestSource>
{
    public void Configure(EntityTypeBuilder<RequestSource> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(c => c.Id)
           .HasConversion(
              id => id.Value,
              value => new RequestSourceId(value))
           .UseIdentityAlwaysColumn();
        builder.Property(e => e.ExternalId).IsRequired().HasMaxLength(100);

        builder.HasIndex(e => e.ExternalId).IsUnique();
    }
}
