using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SellerInformationSeed
{
    public readonly static List<SellerInformation> Default =
    [
        new SellerInformation {
            Id = Guid.Parse("cdd7c66c-49d9-4bcc-8668-7a7aa231f8ec"),
            SellerId = Guid.Parse("0a23fe1c-1912-47c4-9cc8-811a3673c745"),
            ShopName = "Điện thoại thông minh Tấn Cường",
            Address = "123 Đường Lê Lợi, Phường Bến Thành, Quận 1, TP. Hồ Chí Minh, Việt Nam",
            PhoneNumber = "0901234567",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("4f2b9bcf-d684-4fbe-8421-03d9c5bd343b"),
            SellerId = Guid.Parse("78851d40-5eaf-4450-ba72-1464c26e8b51"),
            ShopName = "Laptop 247",
            Address = "456 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh, Việt Nam",
            PhoneNumber = "01234567890",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("aa98fef9-9600-4503-8a97-4d7ed3bba3de"),
            SellerId = Guid.Parse("74231b9a-985a-47db-b589-d62c4ec16041"),
            ShopName = "An Hòa Mobile",
            Address = "86 Nguyễn Thái Học, Phường Cầu Ông Lãnh, Quận 1, TP. Hồ Chí Minh",
            PhoneNumber = "0976411235",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("2444264f-ca71-46bd-8910-cacbe1786d86"),
            SellerId = Guid.Parse("f199a478-7c99-45fa-8f61-d2fd2680310b"),
            ShopName = "IComp Shop",
            Address = "186A Nguyễn Thị Minh Khai, Phường 6, Quận 3, TP. Hồ Chí Minh",
            PhoneNumber = "0987565859",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("13b08223-5a10-48ba-a658-48e87b8fcade"),
            SellerId = Guid.Parse("7b534b70-5bd1-4e6a-8d7f-40edd2decacc"),
            ShopName = "Thế giới điện tử",
            Address = "56 Đường Lạc Long Quân, Phường 3, Quận 11, TP. Hồ Chí Minh",
            PhoneNumber = "0911432646",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("92b8fd44-7354-46d7-8a5b-bc8ec122ae72"),
            SellerId = Guid.Parse("3601814d-abda-444d-85a0-3d7d54d76ed2"),
            ShopName = "Siêu thị công nghệ ",
            Address = "32 Đường Võ Văn Kiệt, Phường 13, Quận 6, TP. Hồ Chí Minh",
            PhoneNumber = "0917652226",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("742e5645-282d-4477-8641-8701919e016e"),
            SellerId = Guid.Parse("b6f15969-3918-4404-9097-41f8d3fa45e8"),
            ShopName = "Thiết bị điện tử Nhật Hạ",
            Address = "72 Lê Thánh Tôn , P. Bến Nghé, Q.1, TP. Hồ Chí Minh",
            PhoneNumber = "0823177160",
            CreatedAt = DateTime.UtcNow,
        },
        new SellerInformation {
            Id = Guid.Parse("20fbca7c-3f95-40c8-a7d1-aa9c7a383926"),
            SellerId = Guid.Parse("6d2ae85c-ef67-48a0-93d8-dbda7704590c"),
            ShopName = "Cửa hàng Thuỳ Uyên",
            Address = "56 Xuân Thủy, Thảo Điền, Quận 2, TP. Hồ Chí Minh",
            PhoneNumber = "0962107532",
            CreatedAt = DateTime.UtcNow,
        },
    ];
}
