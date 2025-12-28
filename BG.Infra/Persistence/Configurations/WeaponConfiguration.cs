using BG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
{
    public void Configure(EntityTypeBuilder<Weapon> builder)
    {
        builder.ToTable("Weapons");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Codename).IsRequired().HasMaxLength(100);

        builder.Property(w => w.SerialNumber).IsRequired().HasMaxLength(100);
        builder.HasIndex(w => w.SerialNumber).IsUnique();

        builder.Property(w => w.Caliber).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Status).HasConversion<string>().HasMaxLength(20);
    }
}