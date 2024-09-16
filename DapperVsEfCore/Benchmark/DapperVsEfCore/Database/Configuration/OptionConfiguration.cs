using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchy.DapperVsEfCore.Database.Configuration;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(DataSchemaConstants.VarCharLength);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(DataSchemaConstants.VarCharLength);
        builder.Property(x => x.IsActive).IsRequired();
    }
}