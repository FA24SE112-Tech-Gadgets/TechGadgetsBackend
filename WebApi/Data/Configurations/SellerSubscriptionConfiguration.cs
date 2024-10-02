using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class SellerSubscriptionConfiguration : IEntityTypeConfiguration<SellerSubscription>
{
    public void Configure(EntityTypeBuilder<SellerSubscription> builder)
    {
        builder.HasData(
            new SellerSubscription
            {
                Id = Guid.Parse("689bf59d-1dbf-41f7-8d21-93f8bb92b999"),
                Name = "Gói Standard",
                Description = "Gửi 10 mail/ngày",
                Type = SellerSubscriptionType.Standard,
                Price = 29000,
                Duration = 30,
                Status = SellerSubscriptionStatus.Active
            },
            new SellerSubscription
            {
                Id = Guid.Parse("76636ac8-444a-478b-8973-ef3da2925c53"),
                Name = "Gói Premium",
                Description = "Gửi 25 mail/ngày",
                Type = SellerSubscriptionType.Standard,
                Price = 49000,
                Duration = 30,
                Status = SellerSubscriptionStatus.Active
            }
        );
    }
}
