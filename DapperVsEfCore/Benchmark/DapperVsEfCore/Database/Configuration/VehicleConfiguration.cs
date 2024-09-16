using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchy.DapperVsEfCore.Database.Configuration;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.LicensePlate).HasMaxLength(DataSchemaConstants.VarCharLength).IsRequired();
        builder.Property(x => x.Price).IsRequired();

        builder
            .HasOne(x => x.Thumbnail)
            .WithOne()
            .HasForeignKey<Vehicle>(x => x.ThumbnailId);

        builder
            .HasOne(x => x.EngineDetails)
            .WithOne()
            .HasForeignKey<Vehicle>(x => x.EngineDetailsId);

        builder
            .HasMany(x => x.DetailImages)
            .WithOne();

        builder
            .HasMany(x => x.DamageImages)
            .WithOne();

        builder
            .HasOne(x => x.Model)
            .WithMany()
            .HasForeignKey(x => x.ModelId);

        builder
            .HasMany(x => x.Options)
            .WithMany();

        builder
            .HasMany(x => x.Tags)
            .WithMany();
    }
}