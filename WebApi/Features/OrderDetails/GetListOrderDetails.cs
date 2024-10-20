using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Data.Entities;

namespace WebApi.Features.OrderDetails;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
public class GetListOrderDetails : ControllerBase
{

}
