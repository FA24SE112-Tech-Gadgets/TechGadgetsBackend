using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;

namespace WebApi.Features.SellerReplies;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Manager)]
public class DeactivateSellerReplyByManager : ControllerBase
{
    [HttpPut("seller-replies/{id}/deactivate")]
    [Tags("Seller Replies")]
    [SwaggerOperation(
        Summary = "Manager Deactivate Seller Reply"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler(Guid id, AppDbContext context)
    {
        var sellerReply = await context.SellerReplies.FirstOrDefaultAsync(g => g.Id == id);
        if (sellerReply is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("sellerReply", "Phản hồi không tồn tại.")
                        .Build();
        }

        if (sellerReply.Status == SellerReplyStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("sellerReply", "Phản hồi đã trong trạng thái inactive.")
                        .Build();
        }

        sellerReply.Status = SellerReplyStatus.Inactive;

        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
