using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.Stats;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
[RequestValidation<Request>]
public class GetSalesRevenueByManager : ControllerBase
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

    [HttpGet("stats/sales-revenue/manager")]
    [Tags("Stats")]
    [SwaggerOperation(
        Summary = "Manager Gets Sales Revenue",
        Description = "API is for manager gets sales revenue"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromQuery] Request request, AppDbContext context)
    {
        var response = await context.SellerOrderItems
                                    .Where(soi => soi.SellerOrder.Status == SellerOrderStatus.Success)
                                    .Where(soi => soi.SellerOrder.UpdatedAt >= DateTime.SpecifyKind(request.Start!.Value, DateTimeKind.Utc)
                                                && soi.SellerOrder.UpdatedAt <= DateTime.SpecifyKind(request.End!.Value, DateTimeKind.Utc))
                                    .Select(s => new
                                    {
                                        Total = s.GadgetQuantity * s.GadgetPrice * (1 - (s.GadgetDiscount != null ? s.GadgetDiscount.DiscountPercentage / 100.0 : 0))
                                    })
                                    .SumAsync(s => s.Total);

        var total = await context.SellerOrderItems
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
