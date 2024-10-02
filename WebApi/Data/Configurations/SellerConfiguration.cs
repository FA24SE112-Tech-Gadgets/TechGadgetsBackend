using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.HasData(
            new Seller
            {
                Id = Guid.Parse("9488d26a-de33-4bf6-b038-be5d1d641940"),
                UserId = Guid.Parse("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"),
                CompanyName = null,
                ShopName = "Cửa hàng Thuỳ Uyên",
                ShippingAddress = "516 Đ. Lê Văn Sỹ, P. 14, Q3, TP. HCM",
                ShopAddress = "37 Đ. Lê Quý Đôn, P. 7, Q3, TP. HCM",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "1779231738",
                PhoneNumber = "0877094491"
            },
            new Seller
            {
                Id = Guid.Parse("cd83c20c-dc5c-4115-87b2-a218e6584301"),
                UserId = Guid.Parse("0a4590ef-a843-4489-94ef-762259b78688"),
                CompanyName = "Công Ty Nhật Hạ",
                ShopName = "Cửa hàng Nhật Hạ",
                ShippingAddress = "76 Đ. Hoa Bằng, Q. Cầu Giấy, TP. Hà Nội",
                ShopAddress = "128 Đ. Nguyễn Phong Sắc, Q. Cầu Giấy, TP. Hà Nội",
                BusinessModel = BusinessModel.Company,
                BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller2.jpg",
                TaxCode = "4067001394",
                PhoneNumber = "0362961803"
            }
        );
    }
}
