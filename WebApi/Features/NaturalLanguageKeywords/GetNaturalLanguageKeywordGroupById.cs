using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.NaturalLanguageKeywords.Mappers;
using WebApi.Features.NaturalLanguageKeywords.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.NaturalLanguageKeywords;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class GetNaturalLanguageKeywordGroupById : ControllerBase
{
    [HttpGet("natual-language-keyword-groups/{id}")]
    [Tags("Natural Language Keywords")]
    [SwaggerOperation(
        Summary = "Get Natural Keyword Group by Id"
    )]
    [ProducesResponseType(typeof(NaturalLanguageKeywordGroupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context, CurrentUserService currentUserService)
    {
        var group = await context.NaturalLanguageKeywordGroups
                                    .Include(g => g.Criteria)
                                        .ThenInclude(c => c.Categories)
                                    .Include(g => g.Criteria.OrderByDescending(c => c.UpdatedAt))
                                        .ThenInclude(c => c.SpecificationKey)
                                    .Include(g => g.NaturalLanguageKeywords.OrderByDescending(k => k.UpdatedAt))
                                    .FirstOrDefaultAsync(g => g.Id == id);

        if (group is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("naturalLanguageKeywordGroup", "Nhóm từ khoá không tồn tại.")
                        .Build();
        }

        return Ok(group.ToNaturalLanguageKeywordGroupResponse());
    }
}
