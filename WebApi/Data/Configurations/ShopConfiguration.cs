using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasData(
            new Shop
            {
                Id = Guid.Parse("1f3c2205-1e9c-4efa-9c6b-57819c114793"),
                Name = "Thế Giới Di Động",
                WebsiteUrl = "https://www.thegioididong.com",
                LogoUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Shops/Thegioididong.jpg",
            },
            new Shop
            {
                Id = Guid.Parse("e5233830-7d0b-45d2-953e-0fe3bb3cc09e"),
                Name = "FPT Shop",
                WebsiteUrl = "https://fptshop.com.vn",
                LogoUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Shops/Fptshop.jpg",
            },
            new Shop
            {
                Id = Guid.Parse("bafc41ac-9b92-4af3-a8da-84cac529be43"),
                Name = "Phong Vũ",
                WebsiteUrl = "https://phongvu.vn",
                LogoUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Shops/Phongvu.jpg",
            }
        );
    }
}
