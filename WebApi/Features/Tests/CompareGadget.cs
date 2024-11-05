using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;

namespace WebApi.Features.Tests;

[ApiController]
[RequestValidation<Request>]
public class CompareGadget : ControllerBase
{
    public new class Request
    {
        public Guid GadgetId1 { get; set; } = default!;
        public Guid GadgetId2 { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GadgetId1)
                .NotEmpty().WithMessage("GadgetId không được để trống");

            RuleFor(r => r.GadgetId2)
                .NotEmpty().WithMessage("GadgetId không được để trống");
        }
    }

    [Tags("Tests")]
    [HttpPost("tests/natural-languages/compare/gadgets")]
    public async Task<IActionResult> Handler(Request request, AppDbContext context)
    {
        var gadget1 = await context.Gadgets.FirstOrDefaultAsync(g => g.Id == request.GadgetId1);
        var gadget2 = await context.Gadgets.FirstOrDefaultAsync(g => g.Id == request.GadgetId2);

        if (gadget1 == null || gadget2 == null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Ít nhất một trong hai sản phẩm không tồn tại")
                        .Build();
        }

        var res = await context.Gadgets
                                       .Where(g => g.Id == gadget2.Id)
                                       .Select(g => new
                                       {
                                           Similarity = 1 - g.Vector!.CosineDistance(gadget1.Vector!)
                                       })
                                       .FirstOrDefaultAsync();

        return Ok(new
        {
            Similarity = res!.Similarity
        });
    }
}
