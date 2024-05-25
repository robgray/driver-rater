namespace DriverRater.Api.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profile");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.RacingId);
        builder.Property(u => u.Name).HasMaxLength(400);

        builder.HasMany<RankedDriver>(u => u.RankedDrivers);
    }
}