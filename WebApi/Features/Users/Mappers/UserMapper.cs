using WebApi.Data.Entities;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Users.Mappers;

public static class UserMapper
{
    public static UserResponse? ToUserResponse(this User? user)
    {
        if (user != null)
        {
            return new UserResponse
            {
                Email = user.Email,
                Id = user.Id,
                Role = user.Role,
                Status = user.Status,
                LoginMethod = user.LoginMethod,
                Manager = user.Manager.ToManagerResponse(),
                Admin = user.Admin.ToAdminResponse(),
                Seller = user.Seller.ToSellerResponse(),
                Customer = user.Customer.ToCustomerResponse(),
                Wallet = user.Wallet.ToWalletResponse(),
            };
        }
        return null;
    }
}
