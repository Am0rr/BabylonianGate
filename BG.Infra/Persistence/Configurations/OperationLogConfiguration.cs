using BG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class OperationLogConfiguration : IEntityTypeConfiguration<OperationLog>
{
    public void Configure(EntityTypeBuilder<OperationLog> builder)
    {
        builder.ToTable("OperationLogs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.CreatedAt).IsRequired();
        builder.Property(l => l.Action).IsRequired().HasMaxLength(50);
        builder.Property(l => l.Details).IsRequired().HasMaxLength(500);
        builder.Property(l => l.RelatedEntityId).IsRequired(false);
        builder.Property(l => l.OperatorId).IsRequired(false);

        builder.HasIndex(l => l.RelatedEntityId);
        builder.HasIndex(l => l.CreatedAt);
        builder.HasIndex(l => l.OperatorId);
    }
}