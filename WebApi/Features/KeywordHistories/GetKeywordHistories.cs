using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Filters;
using WebApi.Common.Paginations;
using WebApi.Data;
using WebApi.Features.KeywordHistories.Models;
using WebApi.Services.Auth;

namespace WebApi.Features.KeywordHistories;

[ApiController]
[JwtValidation]
public class GetKeywordHistories : ControllerBase
{
    [HttpGet("keyword-histories")]
    [Tags("Keyword Histories")]
    [ProducesResponseType(typeof(PagedList<KeywordHistoryResponse>), StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get Keyword Histories",
        Description = """
        This API is to get 5 latest keyword histories
        """
    )]
    public async Task<IActionResult> Handler(AppDbContext context, CurrentUserService currentUserService)
    {
        var user = await currentUserService.GetCurrentUser();

        var keywordHistories = await context.KeywordHistories
                                            .Where(kh => kh.UserId == user!.Id)
                                            .OrderByDescending(kh => kh.CreatedAt)
                                            .Select(kh => new KeywordHistoryResponse
                                            {
                                                Id = kh.Id,
                                                Keyword = kh.Keyword,
                                            })
                                            .Take(5)
                                            .ToListAsync();

        return Ok(keywordHistories);
    }
}
