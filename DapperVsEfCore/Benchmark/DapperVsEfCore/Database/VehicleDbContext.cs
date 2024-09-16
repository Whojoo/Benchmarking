using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore.Database;

public class VehicleDbContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Make> Makes { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<EngineDetails> EngineDetails { get; set; }

    private readonly bool _configured = false;

    public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
    : base(options)
    {
        _configured = true;
    }

    public VehicleDbContext() : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_configured)
        {
            return;
        }

        optionsBuilder.UseSqlServer(DataSchemaConstants.ConnectionString);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DataSchemaConstants.DefaultSchema);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}