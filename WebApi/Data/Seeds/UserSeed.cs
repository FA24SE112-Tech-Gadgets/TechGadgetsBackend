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
                Id = Guid.Parse("9488d26a-de33-4bf6-b038-be5d1d641940"),
                UserId = Guid.Parse("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"),
                CompanyName = null,
                ShopName = "Cửa hàng Thuỳ Uyên",
                ShopAddress = "37 Đ. Lê Quý Đôn, P. 7, Q3, TP. HCM",
                BusinessModel = BusinessModel.Personal,
                BusinessRegistrationCertificateUrl = null,
                TaxCode = "1779231738",
                PhoneNumber = "0877094491"
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
                Id = Guid.Parse("cd83c20c-dc5c-4115-87b2-a218e6584301"),
                UserId = Guid.Parse("0a4590ef-a843-4489-94ef-762259b78688"),
                CompanyName = "Công Ty Nhật Hạ",
                ShopName = "Cửa hàng Nhật Hạ",
                ShopAddress = "128 Đ. Nguyễn Phong Sắc, Q. Cầu Giấy, TP. Hà Nội",
                BusinessModel = BusinessModel.Company,
                BusinessRegistrationCertificateUrl = "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller2.jpg",
                TaxCode = "4067001394",
                PhoneNumber = "0362961803",
            },
            Wallet = new Wallet
            {
                Id = Guid.Parse("28a4b070-e904-4b38-8e58-21a7a7bed48c"),
                Amount = 0,
            }
        },
    ];
}
