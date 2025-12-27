using BG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class OperationLogConfiguration : IEntityTypeConfiguration<OperationLog>
{
    public void Configure(EntityTypeBuilder<OperationLog> builder)
    {
        builder.ToTable("OperationLogs");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Action).IsRequired().HasMaxLength(50);

        builder.Property(w => w.RelatedEntityId).IsRequired(false);
        builder.Property(w => w.Details).IsRequired().HasMaxLength(500);
    }
}