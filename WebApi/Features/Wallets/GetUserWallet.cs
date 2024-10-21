using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Data.Entities;

namespace WebApi.Features.Wallets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer, Role.Seller)]
public class GetUserWallet
{
}
