using Microsoft.EntityFrameworkCore;

namespace GeneralBenchmarks.ResultStructClass;

public class ResultDbContext : DbContext
{
    public DbSet<StorableRequiredObject> StorableRequiredObjects { get; set; }
    public DbSet<StorableObject> StorableObjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(nameof(ResultDbContext));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var storableRequiredObject = modelBuilder.Entity<StorableRequiredObject>();
        storableRequiredObject.HasKey(x => x.Id);
        storableRequiredObject.Property(x => x.Id).ValueGeneratedNever();
        storableRequiredObject.Property(x => x.Name).IsRequired().HasMaxLength(100);
        
        var storableObject = modelBuilder.Entity<StorableObject>();
        storableObject.HasKey(x => x.Id);
        storableObject.Property(x => x.Id).ValueGeneratedNever();
        storableObject.Property(x => x.Name).IsRequired().HasMaxLength(100);
        storableObject.HasOne(x => x.StorableRequired).WithMany().HasForeignKey(x => x.StorableRequiredId);
    }
}