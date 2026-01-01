using BG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BG.Infra.Persistence.Configurations;

public class SoldierConfiguration : IEntityTypeConfiguration<Soldier>
{
    public void Configure(EntityTypeBuilder<Soldier> builder)
    {
        builder.ToTable("Soldiers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(s => s.LastName).IsRequired().HasMaxLength(50);

        builder.Property(s => s.Rank).HasConversion<string>().HasMaxLength(20);
    }
}