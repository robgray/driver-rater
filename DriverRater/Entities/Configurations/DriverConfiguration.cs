namespace DriverRater.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DriverConfiguration : IEntityTypeConfiguration<RankedDriver>
{
    public void Configure(EntityTypeBuilder<RankedDriver> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.RacingId);
        builder.Property(d => d.Name).HasMaxLength(400);
        builder.Property(d => d.Rank).HasConversion(
            d => (int)d,
            d => (DriverRank)d);
        
        builder.HasOne<User>(d => d.RankedBy);
        builder.Property(d => d.DateRankedUtc);
    }
}