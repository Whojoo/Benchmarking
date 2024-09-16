using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchy.DapperVsEfCore.Database.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(DataSchemaConstants.VarCharLength).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(DataSchemaConstants.VarCharLength).IsRequired();
    }
}