using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasData(
            new Category { Id = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Điện thoại" },
            new Category { Id = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Laptop" },
            new Category { Id = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Thiết bị âm thanh" }
            );
    }
}
