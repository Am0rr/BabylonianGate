using BG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class AmmoCrateConfiguration : IEntityTypeConfiguration<AmmoCrate>
{
    public void Configure(EntityTypeBuilder<AmmoCrate> builder)
    {
        builder.ToTable("AmmoCrates");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.LotNumber).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Quantity).IsRequired();
        builder.Property(a => a.Caliber).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Type).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(a => a.LotNumber);
    }
}