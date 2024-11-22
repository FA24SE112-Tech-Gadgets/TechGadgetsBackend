using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Stats;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
[RequestValidation<Request>]
public class GetSalesRevenueBySeller : ControllerBase
{
    public new class Request
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Start)
                .NotNull().WithMessage("Ngày bắt đầu không được để trống");

            RuleFor(r => r.End)
                .NotNull().WithMessage("Ngày cuối cùng không được để trống")
                .GreaterThan(r => r.Start).WithMessage("Ngày cuối cùng phải lớn hơn ngày bắt đầu"); ;

        }
    }

    [HttpGet("stats/sales-revenue/seller")]
    [Tags("Stats")]
    [SwaggerOperation(
        Summary = "Seller Gets Sales Revenue",
        Description = "API is for seller gets sales revenue"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context, CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var response = await context.SellerOrderItems
                                    .Where(soi => soi.SellerOrder.SellerId == currentUser!.Seller!.Id)
                                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                                    .Where(soi => soi.SellerOrder.UpdatedAt >= DateTime.SpecifyKind(request.Start!.Value, DateTimeKind.Utc)
                                                && soi.SellerOrder.UpdatedAt <= DateTime.SpecifyKind(request.End!.Value, DateTimeKind.Utc))
                                    .Select(s => new
                                    {
                                        Total = s.GadgetQuantity * s.GadgetPrice * (1 - (s.GadgetDiscount != null ? s.GadgetDiscount.DiscountPercentage / 100.0 : 0))
                                    })
                                    .SumAsync(s => s.Total);

        var total = await context.SellerOrderItems
                                     .Where(soi => soi.SellerOrder.SellerId == currentUser!.Seller!.Id)
                                     .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                                     .Select(s => new
                                     {
                                         Total = s.GadgetQuantity * s.GadgetPrice * (1 - (s.GadgetDiscount != null ? s.GadgetDiscount.DiscountPercentage / 100.0 : 0))
                                     })
                                     .SumAsync(s => s.Total);

        return Ok(new
        {
            salesRevenue = response,
            totalRevenue = total,
        });
    }
}
