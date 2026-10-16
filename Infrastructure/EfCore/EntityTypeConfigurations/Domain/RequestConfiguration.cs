using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Infrastructure.EfCore.EntityTypeConfigurations.Domain;

internal sealed class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(request => request.Id);
        builder.Property(request => request.Id).HasConversion(
               id => id.Value,
               value => new RequestId(value)).UseIdentityAlwaysColumn();
        builder.Property(request => request.RequestSourceId).IsRequired().HasConversion(
               id => id.Value,
               value => new RequestSourceId(value));
        builder.Property(request => request.ExternalId).IsRequired().HasMaxLength(100);
        builder.Property(request => request.Name).HasMaxLength(200);
        builder.Property(request => request.Description).HasMaxLength(2000);

        builder.HasIndex(request => new
        {
            request.RequestSourceId,
            request.ExternalId
        }).IsUnique();

        builder.HasOne<RequestSource>()
            .WithMany()
            .HasForeignKey(request => request.RequestSourceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
