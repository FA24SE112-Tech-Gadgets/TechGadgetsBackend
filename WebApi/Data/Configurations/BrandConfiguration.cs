using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasMany(e => e.Categories).WithMany(e => e.Brands).UsingEntity<CategoryBrand>();
    }
}
