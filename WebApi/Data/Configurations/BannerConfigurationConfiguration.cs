using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class BannerConfigurationConfiguration : IEntityTypeConfiguration<BannerConfiguration>
{
    public void Configure(EntityTypeBuilder<BannerConfiguration> builder)
    {
        builder.HasData(
            new BannerConfiguration
            {
                Id = Guid.Parse("13f4ef9b-279e-42ab-9993-daa3ed0602fe"),
                MaxNumberOfBanner = 5
            }
        );
    }
}
