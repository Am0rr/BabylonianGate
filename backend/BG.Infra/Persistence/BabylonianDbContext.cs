using BG.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace BG.Infra.Persistence;

public class BabylonianDbContext : DbContext
{
    public BabylonianDbContext(DbContextOptions<BabylonianDbContext> options)
     : base(options)
    {
    }

    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<AmmoCrate> AmmoCrates { get; set; }
    public DbSet<OperationLog> OperationLogs { get; set; }
    public DbSet<Soldier> Soldiers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BabylonianDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}