using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasData(
            new Admin
            {
                Id = Guid.Parse("c3279608-d5f2-4da9-a942-e14573fa41e7"),
                UserId = Guid.Parse("69f7c054-00d2-48f3-9e86-21081f095340"),
                FullName = "Liêu Bình An"
            },
            new Admin
            {
                Id = Guid.Parse("ac09dfb9-0216-4b11-81a6-42c959d95ccb"),
                UserId = Guid.Parse("4808ef8f-f46f-461f-ba41-962e16aec45b"),
                FullName = "Thái Hưng Đạo"
            }
        );
    }
}
