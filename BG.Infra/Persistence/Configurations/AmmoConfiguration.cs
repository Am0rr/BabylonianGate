using BG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class AmmoCrateConfiguration : IEntityTypeConfiguration<AmmoCrate>
{
    public void Configure(EntityTypeBuilder<AmmoCrate> builder)
    {
        builder.ToTable("AmmoCrates");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Quantity).IsRequired();

        builder.Property(w => w.Caliber).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Type).HasConversion<string>().HasMaxLength(20);
    }
}