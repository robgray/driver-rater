namespace HelmetRanker.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.RacingId);
        builder.Property(u => u.Name).HasMaxLength(400);

        builder.HasMany<Driver>(u => u.RankedDrivers);
    }
}