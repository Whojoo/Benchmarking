using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchy.DapperVsEfCore.Database.Configuration;

public class EngineDetailsConfiguration : IEntityTypeConfiguration<EngineDetails>
{
    public void Configure(EntityTypeBuilder<EngineDetails> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Pk).IsRequired();
        builder.Property(e => e.NumberOfCilinders).IsRequired();
        builder.Property(e => e.MaxSpeed).IsRequired();
    }
}