using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }

    public class SpecificationValueRequest
    {
        public Guid SpecificationKeyId { get; set; }
        public Guid? SpecificationUnitId { get; set; }
        public string Value { get; set; } = default!;
    }

    public class GadgetDiscountRequest
    {
        public int? DiscountPercentage { get; set; }
        public DateTime? DiscountExpiredDate { get; set; }
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
        public GadgetDiscountRequest Discount { get; set; }
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
                .LessThanOrEqualTo(1000).WithMessage("Số lượng phải nhỏ hơn 1000");

            RuleFor(r => r.GadgetImages)
                .NotEmpty().WithMessage("GadgetImages không được để trống");

            RuleFor(r => r.GadgetDescriptions)
                .NotEmpty().WithMessage("GadgetDescriptions không được để trống");

            RuleForEach(r => r.GadgetDescriptions).ChildRules(g =>
            {
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

            RuleFor(r => r.Discount)
                .Cascade(CascadeMode.Stop)
                .NotNull().When(r => r.Discount != null) // Check if Discount is provided
                .ChildRules(discount =>
                {
                    discount.RuleFor(d => d.DiscountPercentage)
                        .NotNull().WithMessage("DiscountPercentage không được để trống")
                        .GreaterThan(0).WithMessage("DiscountPercentage phải lớn hơn 0")
                        .LessThanOrEqualTo(90).WithMessage("DiscountPercentage phải nhỏ hơn hoặc bằng 90");

                    discount.RuleFor(d => d.DiscountExpiredDate)
                        .NotNull().WithMessage("DiscountExpiredDate không được để trống")
                        .Must(date => date > DateTime.UtcNow).WithMessage("DiscountExpiredDate phải lớn hơn thời gian hiện tại");
                });
        }
    }

    [HttpPost("gadgets")]
    [Tags("Gadgets")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [SwaggerOperation(Summary = "Create Gadget",
        Description = """
        This API is for creating a gadget

        ### API này không sử dụng được trên swagger, hãy test trên postman

        - GadgetDescriptions:
            - **image** và **text** có thể null
            - **type** có thể là: image, normalText, boldText
            - nếu **type** là image thì field **image** phải chứa file hình ảnh
            - nếu **type** không phải là image thì field **text** phải chứa text tương ứng
            - thứ tự của array sẽ được dùng để tạo index tương ứng, hãy lưu ý

        - Discount:
            - Có thể không cần truyền giảm giá ngay từ lúc create. Còn nếu truyền thì ""discountPercentage** và **discountExpiredDate** không được để trống
            - ""discountPercentage** phải lớn hơn 0 và nhỏ hơn hoặc bẳng 90
            - **discountExpiredDate** phải lớn hơn thời gian hiện tại
        """
    )]
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

        if (!await context.Brands.AnyAsync(b => b.Id == request.BrandId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("brand", "Thương hiệu không tồn tại")
                        .Build();
        }

        if (!await context.Categories.AnyAsync(c => c.Id == request.CategoryId))
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("category", "Thể loại không tồn tại")
                        .Build();
        }

        foreach (var specValue in request.SpecificationValues)
        {
            var specKeyValid = await context.SpecificationKeys
                                    .AnyAsync(s => s.Id == specValue.SpecificationKeyId && s.CategoryId == request.CategoryId);
            if (!specKeyValid)
            {
                throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("specificationKey", $"specificationKey {specValue.SpecificationKeyId} không tồn tại hoặc không hợp lệ với category {request.CategoryId}")
                        .Build();
            }

            if (specValue.SpecificationUnitId.HasValue)
            {
                var specUnitValid = await context.SpecificationUnits
                                    .AnyAsync(u => u.Id == specValue.SpecificationUnitId && u.SpecificationKeyId == specValue.SpecificationKeyId);
                if (!specUnitValid)
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("specificationUnit", $"specificationUnit {specValue.SpecificationUnitId} không tồn tại hoặc không hợp lệ với specificationKey {specValue.SpecificationKeyId}")
                        .Build();
                }
            }
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
                .AddReason("gadgetImages", "Lỗi khi lưu hình ảnh Sản phẩm")
                .Build();
        }

        List<GadgetDescription> gadgetDescriptions = [];
        int index = 0;

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
                        .AddReason("gadgetDescription", "Lỗi khi lưu hình ảnh trong mô tả Sản phẩm")
                        .Build();
                }

                gadgetDescriptions.Add(new GadgetDescription
                {
                    Index = index++,
                    Type = gadgetDescriptionRequest.Type,
                    Value = url
                });
            }
            else
            {
                gadgetDescriptions.Add(new GadgetDescription
                {
                    Index = index++,
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
            IsForSale = true,
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

        if (request.Discount is { DiscountPercentage: int discountPercentage, DiscountExpiredDate: DateTime discountExpiredDate })
        {
            gadgetToCreate.GadgetDiscounts.Add(new GadgetDiscount
            {
                DiscountPercentage = discountPercentage,
                ExpiredDate = discountExpiredDate,
                Status = GadgetDiscountStatus.Active,
                CreatedAt = DateTime.UtcNow,
            });
        }

        context.Gadgets.Add(gadgetToCreate);
        await context.SaveChangesAsync();

        return Created();
    }

}
