using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shiko_profile_provider.Domain.Entities;

namespace shiko_profile_provider.Infrastructure.Persistence.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.ProfileImage)
            .HasMaxLength(500);

        builder.HasIndex(x => x.Id);
    }
}
