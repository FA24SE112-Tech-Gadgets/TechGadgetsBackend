using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
public class GetSpecValueAsString : ControllerBase
{
    public new class Request
    {
        public string Text1 { get; set; } = default!;
        public string Text2 { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Text1)
                .NotEmpty().WithMessage("Input không được để trống");

            RuleFor(r => r.Text2)
                .NotEmpty().WithMessage("Input không được để trống");
        }
    }

    [Tags("Natural Language")]
    [HttpPost("natural-languages/compare/get-spec/{gadgetId}")]
    public async Task<IActionResult> Handler(AppDbContext context, Guid gadgetId)
    {
        var gadget = await context.Gadgets
                        .Include(g => g.SpecificationValues)
                            .ThenInclude(sv => sv.SpecificationUnit)
                        .Include(g => g.SpecificationValues)
                            .ThenInclude(sv => sv.SpecificationKey)
                        .FirstOrDefaultAsync(g => g.Id == gadgetId);

        string s = gadget!.Name + " ";
        foreach (var specValue in gadget.SpecificationValues)
        {
            if (specValue.SpecificationUnit != null)
            {
                s += specValue.SpecificationKey.Name + " " + specValue.Value + " " + specValue.SpecificationUnit.Name + " ";
            }
            else
            {
                s += specValue.SpecificationKey.Name + " " + specValue.Value + " ";
            }
        }

        return Ok(s);
    }
}
