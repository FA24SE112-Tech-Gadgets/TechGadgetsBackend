using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class UserSeed
{
    public readonly static List<User> Default =
    [
        new User {
            Id = Guid.Parse("69f7c054-00d2-48f3-9e86-21081f095340"),
            Email = "admin1@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Admin,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Admin = new Admin
            {
                Id = Guid.Parse("c3279608-d5f2-4da9-a942-e14573fa41e7"),
                UserId = Guid.Parse("69f7c054-00d2-48f3-9e86-21081f095340"),
                FullName = "Liêu Bình An"
            }
        },
        new User {
            Id = Guid.Parse("4808ef8f-f46f-461f-ba41-962e16aec45b"),
            Email = "admin2@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Admin,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Admin = new Admin
            {
                Id = Guid.Parse("ac09dfb9-0216-4b11-81a6-42c959d95ccb"),
                UserId = Guid.Parse("4808ef8f-f46f-461f-ba41-962e16aec45b"),
                FullName = "Thái Hưng Đạo"
            }
        },
        new User {
            Id = Guid.Parse("27a15668-0d9e-4276-a0df-791b7dfeed9e"),
            Email = "manager1@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Manager,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Manager = new Manager
            {
                Id = Guid.Parse("2723dacf-59e1-4d66-bf90-5813432c79a8"),
                UserId = Guid.Parse("27a15668-0d9e-4276-a0df-791b7dfeed9e"),
                FullName = "Mã Duy Hình"
            }
        },
        new User {
            Id = Guid.Parse("638eadf4-a17f-4f16-a9dd-6d12a5b5a80a"),
            Email = "manager2@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Manager,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Manager = new Manager
            {
                Id = Guid.Parse("0ca3bd29-b37e-49f2-8fb6-7c44efca1745"),
                UserId = Guid.Parse("638eadf4-a17f-4f16-a9dd-6d12a5b5a80a"),
                FullName = "Hình Trọng Hùng"
            }
        },
        new User {
            Id = Guid.Parse("5a57223a-6e7d-401b-a19e-bf9282db69fe"),
            Email = "customer1@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Customer,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Customer = new Customer
            {
                Id = Guid.Parse("d61a23a0-dbfe-429f-9c1f-5e0b955beff9"),
                UserId = Guid.Parse("5a57223a-6e7d-401b-a19e-bf9282db69fe"),
                FullName = "Nguỵ Chi Mai",
                Address = "561A Điện Biên Phủ, Quận Bình Thạnh, TP.HCM",
                PhoneNumber = "0967310804",
                Cart = new Cart
                {
                    Id = Guid.Parse("46975434-37e8-4585-b7b5-2d7448a24517")
                }
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("62fa1037-79b4-48e2-ac49-9bbf69d41420"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("8d8707e4-299d-450b-bc5c-f8ab49504fce"),
            Email = "customer2@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Customer,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Customer = new Customer
            {
                Id = Guid.Parse("f9ff20a7-05b3-4dbc-b260-26ef16db8513"),
                UserId = Guid.Parse("8d8707e4-299d-450b-bc5c-f8ab49504fce"),
                FullName = "Lê Thuý Hiền",
                Address = "88 Song Hành, P. An Phú , Quận 2 , TP. HCM",
                PhoneNumber = "0907110400",
                Cart = new Cart
                {
                    Id = Guid.Parse("f05bb2a5-d5e9-417d-a793-5c8333e41669")
                }
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("43e2028e-e7a7-46cb-94bd-fcbc28d66a48"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"),
            Email = "seller1@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("0a23fe1c-1912-47c4-9cc8-811a3673c745"),
                UserId = Guid.Parse("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"),
                CompanyName = null,
                ShopName = "Điện thoại thông minh Tấn Cường",
                ShopAddress = "123 Đường Lê Lợi, Phường Bến Thành, Quận 1, TP. Hồ Chí Minh, Việt Nam",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "3416437196",
                PhoneNumber = "0901234567"
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("3da483f7-1dd7-4bb3-8c9e-f400b0ca6fb0"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("0a4590ef-a843-4489-94ef-762259b78688"),
            Email = "seller2@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("78851d40-5eaf-4450-ba72-1464c26e8b51"),
                UserId = Guid.Parse("0a4590ef-a843-4489-94ef-762259b78688"),
                CompanyName = null,
                ShopName = "Laptop 247",
                ShopAddress = "456 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh, Việt Nam",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "4826305858",
                PhoneNumber = "01234567890",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("28a4b070-e904-4b38-8e58-21a7a7bed48c"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("7d5d5e29-00da-4b23-bcf5-10f5c54bb83c"),
            Email = "seller3@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("74231b9a-985a-47db-b589-d62c4ec16041"),
                UserId = Guid.Parse("7d5d5e29-00da-4b23-bcf5-10f5c54bb83c"),
                CompanyName = "Công ty An Hòa",
                ShopName = "An Hòa Mobile",
                ShopAddress = "86 Nguyễn Thái Học, Phường Cầu Ông Lãnh, Quận 1, TP. Hồ Chí Minh",
                BusinessModel = BusinessModel.Company,
                BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller3.png",
                TaxCode = "5653560627",
                PhoneNumber = "0976411235",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("5f3e26af-ee14-4acb-bb03-a75302674a62"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("a7a70bc6-9519-4f3a-b049-07527ca86b14"),
            Email = "seller4@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("f199a478-7c99-45fa-8f61-d2fd2680310b"),
                UserId = Guid.Parse("a7a70bc6-9519-4f3a-b049-07527ca86b14"),
                CompanyName = "Công ty ICom",
                ShopName = "IComp Shop",
                ShopAddress = "186A Nguyễn Thị Minh Khai, Phường 6, Quận 3, TP. Hồ Chí Minh",
                BusinessModel = BusinessModel.Company,
                BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller4.jpg",
                TaxCode = "1959235336",
                PhoneNumber = "0987565859",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("52bcab14-ed53-4260-91b0-e5e28598b6a6"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("f5595980-35a4-4a0b-9a75-35bf36dec742"),
            Email = "seller5@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("7b534b70-5bd1-4e6a-8d7f-40edd2decacc"),
                UserId = Guid.Parse("f5595980-35a4-4a0b-9a75-35bf36dec742"),
                CompanyName = null,
                ShopName = "Thế giới điện tử",
                ShopAddress = "56 Đường Lạc Long Quân, Phường 3, Quận 11, TP. Hồ Chí Minh",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "9218932470",
                PhoneNumber = "0911432646",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("f23e879e-be70-4a04-8c5b-fab377905f26"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("b1c010d1-f028-4df1-b464-3166a0cd1a67"),
            Email = "seller6@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("3601814d-abda-444d-85a0-3d7d54d76ed2"),
                UserId = Guid.Parse("b1c010d1-f028-4df1-b464-3166a0cd1a67"),
                CompanyName = null,
                ShopName = "Siêu thị công nghệ",
                ShopAddress = "32 Đường Võ Văn Kiệt, Phường 13, Quận 6, TP. Hồ Chí Minh",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "1787808749",
                PhoneNumber = "0917652226",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("47c3589b-5be6-4dd9-a543-cc987a987057"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("c67e1c72-7d39-4201-b243-6e2971686179"),
            Email = "seller7@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("b6f15969-3918-4404-9097-41f8d3fa45e8"),
                UserId = Guid.Parse("c67e1c72-7d39-4201-b243-6e2971686179"),
                CompanyName = "Công Ty Nhật Hạ",
                ShopName = "Thiết bị điện tử Nhật Hạ",
                ShopAddress = "72 Lê Thánh Tôn , P. Bến Nghé, Q.1, TP. Hồ Chí Minh",
                BusinessModel = BusinessModel.BusinessHousehold,
                BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller7.jpg",
                TaxCode = "2657519007",
                PhoneNumber = "0823177160",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("f3e57336-fb4a-4e66-b064-8664c94fb008"),
                Amount = 0,
            }
        },
        new User {
            Id = Guid.Parse("20da5cc1-940d-4971-b12c-87174732c3d7"),
            Email = "seller8@gmail.com",
            Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
            Role = Role.Seller,
            LoginMethod = LoginMethod.Default,
            Status = UserStatus.Active,
            Seller = new Seller
            {
                Id = Guid.Parse("6d2ae85c-ef67-48a0-93d8-dbda7704590c"),
                UserId = Guid.Parse("20da5cc1-940d-4971-b12c-87174732c3d7"),
                CompanyName = null,
                ShopName = "Cửa hàng Thuỳ Uyên",
                ShopAddress = "56 Xuân Thủy, Thảo Điền, Quận 2, TP. Hồ Chí Minh",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "6623839794",
                PhoneNumber = "0962107532",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("beaeeebc-8ccf-4658-9ecd-c4be447c0cca"),
                Amount = 0,
            }
        },
    ];
}
