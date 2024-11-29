using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Users.Mappers;
using WebApi.Features.Users.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.Users;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Admin)]
[RequestValidation<Request>]
public class GetListUsers : ControllerBase
{
    public new class Request : PagedRequest
    {
        public Role? Role { get; set; } = default!;
        public UserStatus? Status { get; set; }
        public string? Name { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Role)
                .Must(role => role != Role.Admin)
                .When(r => r.Role.HasValue)
                .WithMessage("Không thể filter theo role Admin");

            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Tên không được để trống")
                .When(r => r.Name != null); // Chỉ validate nếu Name được truyền
        }
    }

    [HttpGet("users")]
    [Tags("Users")]
    [SwaggerOperation(
        Summary = "Get List Users",
        Description = "API is for Admin get list of users. Note:" +
                            "<br>&nbsp; - Các Admin không thể thấy lẫn nhau => Không thể tìm thấy acc Admin." +
                            "<br>&nbsp; - Nếu không truyền Status sẽ hiểu là lấy all Status" +
                            "<br>&nbsp; - Role nếu không truyền thì sẽ lấy all Role trừ Role 'Admin'" +
                            "<br>&nbsp; - Name sẽ được kiếm theo FullName(Customer, Manager) và ShopName(Seller)." +
                            "<br>&nbsp; - User bị Inactive thì vẫn xem users được."
    )]
    [ProducesResponseType(typeof(PagedList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var query = context.Users.AsQueryable();

        if (!request.Role.HasValue)
        {
            query = query.Where(u => u.Role != Role.Admin);
        }
        else
        {
            query = query.Where(u => u.Role == request.Role);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(u => u.Status == request.Status.Value);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(u =>
                (u.Role == Role.Customer && u.Customer!.FullName.ToLower().Contains(request.Name.ToLower())) ||
                (u.Role == Role.Manager && u.Manager!.FullName.ToLower().Contains(request.Name.ToLower())) ||
                (u.Role == Role.Seller && u.Seller!.ShopName.ToLower().Contains(request.Name.ToLower()))
            );
        }

        var usersResponse = await query
        .Select(u => new UserResponse
        {
            Id = u.Id,
            Email = u.Email,
            Role = u.Role,
            Status = u.Status,
            Manager = u.Manager.ToManagerResponse(),
            Seller = u.Seller.ToSellerResponse(),
            Customer = u.Customer.ToCustomerResponse(),
            LoginMethod = u.LoginMethod,
        })
        .ToPagedListAsync(request);

        return Ok(usersResponse);
    }
}
