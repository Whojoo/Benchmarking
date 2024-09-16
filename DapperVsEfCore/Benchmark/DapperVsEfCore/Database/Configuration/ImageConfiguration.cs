using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchy.DapperVsEfCore.Database.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SmallUrl).HasMaxLength(DataSchemaConstants.VarCharLength).IsRequired();
        builder.Property(x => x.MediumUrl).HasMaxLength(DataSchemaConstants.VarCharLength).IsRequired();
        builder.Property(x => x.OriginalUrl).HasMaxLength(DataSchemaConstants.VarCharLength).IsRequired();
    }
}