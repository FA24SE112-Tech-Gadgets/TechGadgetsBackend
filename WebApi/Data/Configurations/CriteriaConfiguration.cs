using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class CriteriaConfiguration : IEntityTypeConfiguration<Criteria>
{
    public void Configure(EntityTypeBuilder<Criteria> builder)
    {
        builder.HasMany(e => e.Categories).WithMany(e => e.Criteria).UsingEntity<CriteriaCategory>();
    }
}
