using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Auth;
using WebApi.Services.Embedding;
using WebApi.Services.Storage;

namespace WebApi.Features.Gadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Seller)]
[RequestValidation<Request>]
public class CreateGadget : ControllerBase
{
    public class GadgetDescriptionRequest
    {
        public IFormFile? Image { get; set; }
        public string? Text { get; set; }
        public GadgetDescriptionType Type { get; set; }
        public int Index { get; set; }
    }

    public class SpecificationValueRequest
    {
        public Guid SpecificationKeyId { get; set; }
        public Guid? SpecificationUnitId { get; set; }
        public string Value { get; set; } = default!;
    }

    public new class Request
    {
        public Guid BrandId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Price { get; set; }
        public IFormFile ThumbnailUrl { get; set; } = default!;
        public Guid CategoryId { get; set; } = default!;
        public string Condition { get; set; } = default!;
        public int Quantity { get; set; }
        public ICollection<IFormFile> GadgetImages { get; set; } = [];
        public ICollection<GadgetDescriptionRequest> GadgetDescriptions { get; set; } = [];
        public ICollection<SpecificationValueRequest> SpecificationValues { get; set; } = [];
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.BrandId)
                .NotEmpty().WithMessage("BrandId không được để trống");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Tên không được để trống");

            RuleFor(r => r.Price)
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0")
                .LessThanOrEqualTo(100_000_000).WithMessage("Giá phải nhỏ hơn 100 triệu");

            RuleFor(r => r.ThumbnailUrl)
                .NotNull().WithMessage("Thumbnail không được để trống");

            RuleFor(r => r.CategoryId)
                .NotEmpty().WithMessage("CategoryId không được để trống");

            RuleFor(r => r.Condition)
                .NotEmpty().WithMessage("Tình trạng không được để trống");

            RuleFor(r => r.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng phải lớn hơn hoặc bằng 0")
                .LessThanOrEqualTo(100_000_000).WithMessage("Số lượng phải nhỏ hơn 1000");

            RuleFor(r => r.GadgetImages)
                .NotEmpty().WithMessage("GadgetImages không được để trống");

            RuleFor(r => r.GadgetDescriptions)
                .NotEmpty().WithMessage("GadgetDescriptions không được để trống");

            RuleForEach(r => r.GadgetDescriptions).ChildRules(g =>
            {
                g.RuleFor(x => x.Index)
                    .GreaterThanOrEqualTo(0).WithMessage("Index phải lớn hơn hoặc bằng 0");

                g.RuleFor(x => x.Image)
                    .NotNull()
                    .When(x => x.Type == GadgetDescriptionType.Image)
                    .WithMessage("Image không được null khi Type là Image");

                g.RuleFor(x => x.Text)
                    .NotNull()
                    .When(x => x.Type != GadgetDescriptionType.Image)
                    .WithMessage("Text không được null khi Type không phải Image");
            });

            RuleFor(r => r.SpecificationValues)
                .NotEmpty().WithMessage("SpecificationValues không được để trống");

            RuleForEach(x => x.SpecificationValues).ChildRules(specificationValue =>
            {
                specificationValue.RuleFor(x => x.SpecificationKeyId)
                    .NotEmpty()
                    .WithMessage("SpecificationKeyId không được để trống");

                specificationValue.RuleFor(x => x.Value)
                    .NotEmpty()
                    .WithMessage("Value không được để trống");
            });

        }
    }

    [HttpPost("gadgets")]
    [Tags("Gadgets")]
    //[ProducesResponseType(typeof(GadgetResponse), StatusCodes.Status201Created)]
    [SwaggerOperation(Summary = "Create Gadget", Description = "This API is for creating a gadget")]
    public async Task<IActionResult> Handler([FromForm] Request request,
        AppDbContext context, GoogleStorageService storageService, CurrentUserService currentUserService, EmbeddingService embeddingService)
    {
        var user = await currentUserService.GetCurrentUser();

        if (user!.Seller is null)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Seller chưa được kích hoạt")
                .Build();
        }

        if (user.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_03)
                .AddReason("seller", "Tài khoản Seller đã bị khoá")
                .Build();
        }

        //up hinh de cuoi cung
        string? thumbnailUrl = null;
        try
        {
            thumbnailUrl = await storageService.UploadFileToCloudStorage(request.ThumbnailUrl, Guid.NewGuid().ToString());
        }
        catch (Exception)
        {
            if (thumbnailUrl != null)
            {
                await storageService.DeleteFileFromCloudStorage(thumbnailUrl);
            }
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WES_00)
                .AddReason("thumbnailUrl", "Lỗi khi lưu thumbnail")
                .Build();
        }

        List<GadgetImage> gadgetImages = [];
        try
        {
            foreach (var image in request.GadgetImages)
            {
                var url = await storageService.UploadFileToCloudStorage(image, Guid.NewGuid().ToString());
                gadgetImages.Add(new GadgetImage
                {
                    ImageUrl = url
                });
            }
        }
        catch (Exception)
        {
            foreach (var gadgetImage in gadgetImages)
            {
                if (gadgetImage.ImageUrl != null)
                {
                    await storageService.DeleteFileFromCloudStorage(gadgetImage.ImageUrl);
                }
            }
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WES_00)
                .AddReason("gadgetImages", "Lỗi khi lưu hình ảnh thiết bị")
                .Build();
        }

        List<GadgetDescription> gadgetDescriptions = [];
        foreach (var gadgetDescriptionRequest in request.GadgetDescriptions)
        {
            if (gadgetDescriptionRequest.Type == GadgetDescriptionType.Image)
            {
                string? url = null;
                try
                {
                    url = await storageService.UploadFileToCloudStorage(gadgetDescriptionRequest.Image!, Guid.NewGuid().ToString());
                }
                catch (Exception)
                {
                    if (url != null)
                    {
                        await storageService.DeleteFileFromCloudStorage(url);
                    }
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WES_00)
                        .AddReason("gadgetDescription", "Lỗi khi lưu hình ảnh trong mô tả thiết bị")
                        .Build();
                }
                gadgetDescriptions.Add(new GadgetDescription
                {
                    Index = gadgetDescriptionRequest.Index,
                    Type = gadgetDescriptionRequest.Type,
                    Value = url
                });
            }
            else
            {
                gadgetDescriptions.Add(new GadgetDescription
                {
                    Index = gadgetDescriptionRequest.Index,
                    Type = gadgetDescriptionRequest.Type,
                    Value = gadgetDescriptionRequest.Text!
                });
            }
        }

        var gadgetToCreate = new Gadget
        {
            SellerId = user.Seller.Id,
            BrandId = request.BrandId,
            Name = request.Name,
            Price = request.Price,
            ThumbnailUrl = thumbnailUrl,
            CategoryId = request.CategoryId,
            Status = GadgetStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Condition = request.Condition,
            ConditionVector = await embeddingService.GetEmbedding(request.Condition),
            NameVector = await embeddingService.GetEmbedding(request.Name),
            Quantity = request.Quantity,
            GadgetImages = gadgetImages,
            GadgetDescriptions = gadgetDescriptions,
            SpecificationValues = await Task.WhenAll(request.SpecificationValues.Select(async s => new SpecificationValue
            {
                SpecificationKeyId = s.SpecificationKeyId,
                SpecificationUnitId = s.SpecificationUnitId,
                Value = s.Value,
                Vector = await embeddingService.GetEmbedding(s.Value)
            }))
        };

        context.Gadgets.Add(gadgetToCreate);
        await context.SaveChangesAsync();

        return Created();
    }

}
