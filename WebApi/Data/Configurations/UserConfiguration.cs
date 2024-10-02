using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id = Guid.Parse("69f7c054-00d2-48f3-9e86-21081f095340"),
                Email = "admin1@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Admin,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("4808ef8f-f46f-461f-ba41-962e16aec45b"),
                Email = "admin2@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Admin,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("27a15668-0d9e-4276-a0df-791b7dfeed9e"),
                Email = "manager1@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Manager,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("638eadf4-a17f-4f16-a9dd-6d12a5b5a80a"),
                Email = "manager2@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Manager,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("5a57223a-6e7d-401b-a19e-bf9282db69fe"),
                Email = "customer1@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Customer,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("8d8707e4-299d-450b-bc5c-f8ab49504fce"),
                Email = "customer2@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Customer,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"),
                Email = "seller1@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Seller,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            },
            new User
            {
                Id = Guid.Parse("0a4590ef-a843-4489-94ef-762259b78688"),
                Email = "seller2@gmail.com",
                Password = "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U",
                Role = Role.Seller,
                LoginMethod = LoginMethod.Default,
                Status = UserStatus.Active,
            }
        );
    }
}
