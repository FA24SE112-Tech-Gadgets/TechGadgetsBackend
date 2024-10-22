﻿using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SellerApplicationSeed
{
    public readonly static List<SellerApplication> Default =
    [
new SellerApplication { Id = Guid.Parse("c11f145b-ea4d-437b-8d05-785029327ce6"), UserId = Guid.Parse("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"), CompanyName = null, ShopName = "Điện thoại thông minh Tấn Cường", ShopAddress = "123 Đường Lê Lợi, Phường Bến Thành, Quận 1, TP. Hồ Chí Minh, Việt Nam", BusinessModel = BusinessModel.Personal, BusinessRegistrationCertificateUrl = null, TaxCode = "3416437196", PhoneNumber = "901234567", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("0303a2a3-9b29-487c-9ecf-764e3952881e"), UserId = Guid.Parse("0a4590ef-a843-4489-94ef-762259b78688"), CompanyName = null, ShopName = "Laptop 247", ShopAddress = "456 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh, Việt Nam", BusinessModel = BusinessModel.Personal, BusinessRegistrationCertificateUrl = null, TaxCode = "4826305858", PhoneNumber = "1234567890", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("eb470dbb-78fd-410b-a52c-8d46437eca57"), UserId = Guid.Parse("7d5d5e29-00da-4b23-bcf5-10f5c54bb83c"), CompanyName = "Công ty An Hòa", ShopName = "An Hòa Mobile", ShopAddress = "86 Nguyễn Thái Học, Phường Cầu Ông Lãnh, Quận 1, TP. Hồ Chí Minh", BusinessModel = BusinessModel.Company, BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller3.png", TaxCode = "5653560627", PhoneNumber = "976411235", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("aca15075-631d-4ff4-bf6a-fd882182efe8"), UserId = Guid.Parse("a7a70bc6-9519-4f3a-b049-07527ca86b14"), CompanyName = "Công ty ICom", ShopName = "IComp Shop", ShopAddress = "186A Nguyễn Thị Minh Khai, Phường 6, Quận 3, TP. Hồ Chí Minh", BusinessModel = BusinessModel.Company, BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller4.jpg", TaxCode = "1959235336", PhoneNumber = "987565859", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("a9ff33a1-0494-41d6-bb7f-e64a966c5735"), UserId = Guid.Parse("f5595980-35a4-4a0b-9a75-35bf36dec742"), CompanyName = null, ShopName = "Thế giới điện tử", ShopAddress = "56 Đường Lạc Long Quân, Phường 3, Quận 11, TP. Hồ Chí Minh", BusinessModel = BusinessModel.Personal, BusinessRegistrationCertificateUrl = null, TaxCode = "9218932470", PhoneNumber = "911432646", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("a9a039dd-e293-4245-b580-98ec79d4848e"), UserId = Guid.Parse("b1c010d1-f028-4df1-b464-3166a0cd1a67"), CompanyName = null, ShopName = "Siêu thị công nghệ ", ShopAddress = "32 Đường Võ Văn Kiệt, Phường 13, Quận 6, TP. Hồ Chí Minh", BusinessModel = BusinessModel.Personal, BusinessRegistrationCertificateUrl = null, TaxCode = "1787808749", PhoneNumber = "917652226", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("f329755b-4816-46d3-975d-3d1d812cb0c3"), UserId = Guid.Parse("c67e1c72-7d39-4201-b243-6e2971686179"), CompanyName = "Công Ty Nhật Hạ", ShopName = "Thiết bị điện tử Nhật Hạ", ShopAddress = "72 Lê Thánh Tôn , P. Bến Nghé, Q.1, TP. Hồ Chí Minh", BusinessModel = BusinessModel.BusinessHousehold, BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller7.jpg", TaxCode = "2657519007", PhoneNumber = "823177160", Status = SellerApplicationStatus.Approved },
new SellerApplication { Id = Guid.Parse("a4719ea5-695c-4b72-ae1c-7c2b26c63c8c"), UserId = Guid.Parse("20da5cc1-940d-4971-b12c-87174732c3d7"), CompanyName = null, ShopName = "Cửa hàng Thuỳ Uyên", ShopAddress = "56 Xuân Thủy, Thảo Điền, Quận 2, TP. Hồ Chí Minh", BusinessModel = BusinessModel.Personal, BusinessRegistrationCertificateUrl = null, TaxCode = "6623839794", PhoneNumber = "962107532", Status = SellerApplicationStatus.Approved },
    ];
}