using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class CustomerSubscriptionConfiguration : IEntityTypeConfiguration<CustomerSubscription>
{
    public void Configure(EntityTypeBuilder<CustomerSubscription> builder)
    {
        builder.HasData(
            new CustomerSubscription
            {
                Id = Guid.Parse("53e4a057-7dc4-4c0d-9ef6-d8f6b97fb7d8"),
                Name = "Gói Standard",
                Description = "Sử dụng tính năng tìm kiếm bằng ngôn ngữ tự nhiên không giới hạn",
                Type = CustomerSubscriptionType.Standard,
                Price = 29000,
                Duration = 30,
                Status = CustomerSubscriptionStatus.Active
            }
        );
    }
}
