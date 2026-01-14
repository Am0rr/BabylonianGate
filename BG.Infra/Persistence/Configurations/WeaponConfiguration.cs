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

        builder.Property(w => w.CreatedAt).IsRequired();
        builder.Property(w => w.Codename).IsRequired().HasMaxLength(100);
        builder.Property(w => w.SerialNumber).IsRequired().HasMaxLength(100);
        builder.Property(w => w.Caliber).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Type).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(w => w.Condition).IsRequired();
        builder.Property(w => w.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(w => w.IssuedToSoldierId).IsRequired(false);

        builder.HasIndex(w => w.SerialNumber).IsUnique();
        builder.HasIndex(w => w.IssuedToSoldierId);
    }
}