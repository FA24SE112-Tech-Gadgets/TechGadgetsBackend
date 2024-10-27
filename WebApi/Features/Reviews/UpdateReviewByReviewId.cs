﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;

namespace WebApi.Features.Reviews;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
[RequestValidation<Request>]
public class UpdateReviewByReviewId : ControllerBase
{
    public new class Request
    {
        public int? Rating { get; set; }
        public string? Content { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Content)
                .NotEmpty()
                .WithMessage("Nội dung đánh giá không được để trống")
                .When(r => r.Content != null); // Chỉ validate nếu Content được truyền

            RuleFor(r => r.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Đánh giá phải nằm trong khoảng từ 0 đến 5")
                .When(r => r.Rating != null); // Chỉ validate nếu Rating được truyền
        }
    }

    [HttpPatch("review/{reviewId}")]
    [Tags("Reviews")]
    [SwaggerOperation(
        Summary = "Customer Update Review By ReviewId",
        Description = "API is for customer update review by reviewId. Note:" +
                            "<br>&nbsp; - Rating là số nguyên từ 0 - 5." +
                            "<br>&nbsp; - Nội dung không được để trống." +
                            "<br>&nbsp; - Truyền field nào update field đó." +
                            "<br>&nbsp; - Chỉ được thay đổi đánh giá 1 lần." +
                            "<br>&nbsp; - Chỉ được cập nhật đánh giá của bản thân."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, [FromRoute] Guid reviewId, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("review", "Không tìm thấy đánh giá này.")
            .Build();
        }

        if (review.CustomerId != currentUser!.Customer!.Id)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("review", "Người dùng không đủ thẩm quyền để cập nhật đánh giá này.")
            .Build();
        }

        if (review.Status != ReviewStatus.Active)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("review", "Đánh giá này đã bị chặn.")
            .Build();
        }

        if (review.CreatedAt <= DateTime.UtcNow.AddMinutes(-10))
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("review", "Đã quá thời gian sửa đánh giá (10 phút).")
            .Build();
        }

        bool isUpdated = review.CreatedAt != review.UpdatedAt;

        if (isUpdated)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_01)
            .AddReason("review", $"Sản phẩm này đã sửa đánh giá rồi.")
            .Build();
        }

        if (request.Rating != null)
        {
            review.Rating = (int)request.Rating;
            if (request.Rating >= 3)
            {
                review.IsPositive = true;
            } else
            {
                review.IsPositive = false;
            }
        }

        if (request.Content != null)
        {
            review.Content = request.Content;
        }

        if (request.Rating != null || request.Content != null)
        {
            review.UpdatedAt = DateTime.UtcNow;
        }

        context.Reviews.Update(review);
        await context.SaveChangesAsync();

        return Ok();
    }
}