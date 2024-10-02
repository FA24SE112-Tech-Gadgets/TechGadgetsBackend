using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasData(
            new Manager
            {
                Id = Guid.Parse("0ca3bd29-b37e-49f2-8fb6-7c44efca1745"),
                UserId = Guid.Parse("638eadf4-a17f-4f16-a9dd-6d12a5b5a80a"),
                FullName = "Hình Trọng Hùng"
            },
            new Manager
            {
                Id = Guid.Parse("2723dacf-59e1-4d66-bf90-5813432c79a8"),
                UserId = Guid.Parse("27a15668-0d9e-4276-a0df-791b7dfeed9e"),
                FullName = "Mã Duy Hình"
            }
        );
    }
}
