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
public class UpdateGadget : ControllerBase
{
    public class GadgetDescriptionRequest
    {
        public Guid? Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? Text { get; set; }
        public GadgetDescriptionType Type { get; set; }
    }

    public class SpecificationValueRequest
    {
        public Guid? Id { get; set; }
        public Guid SpecificationKeyId { get; set; }
        public Guid? SpecificationUnitId { get; set; }
        public string Value { get; set; } = default!;
    }

    public class GadgetImageRequest
    {
        public Guid? Id { get; set; }
        public IFormFile? Image { get; set; }
    }

    public new class Request
    {
        public Guid BrandId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Price { get; set; }
        public IFormFile? ThumbnailUrl { get; set; } = default!;
        public Guid CategoryId { get; set; } = default!;
        public string Condition { get; set; } = default!;
        public int Quantity { get; set; }
        public ICollection<GadgetImageRequest> GadgetImages { get; set; } = [];
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

            RuleFor(r => r.CategoryId)
                .NotEmpty().WithMessage("CategoryId không được để trống");

            RuleFor(r => r.Condition)
                .NotEmpty().WithMessage("Tình trạng không được để trống");

            RuleFor(r => r.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng phải lớn hơn hoặc bằng 0")
                .LessThanOrEqualTo(1000).WithMessage("Số lượng phải nhỏ hơn 1000");

            RuleFor(r => r.GadgetImages)
                .NotEmpty().WithMessage("GadgetImages không được để trống");

            RuleForEach(r => r.GadgetImages).ChildRules(g =>
            {
                g.RuleFor(x => x.Image)
                    .NotNull()
                    .When(x => x.Id == null)
                    .WithMessage("Image không được null khi Id là null");
            });

            RuleFor(r => r.GadgetDescriptions)
                .NotEmpty().WithMessage("GadgetDescriptions không được để trống");

            RuleForEach(r => r.GadgetDescriptions).ChildRules(g =>
            {
                g.RuleFor(x => x.Image)
                    .NotNull()
                    .When(x => x.Type == GadgetDescriptionType.Image && x.Id == null)
                    .WithMessage("Image không được null khi Type là Image");

                g.RuleFor(x => x.Text)
                    .NotNull()
                    .When(x => x.Type != GadgetDescriptionType.Image && x.Id == null)
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


    [HttpPut("gadgets/{id}")]
    [Tags("Gadgets")]
    [SwaggerOperation(Summary = "Seller Update Gadget",
        Description = """
        API for Seller to update gadge

        ### API này không sử dụng được trên swagger, hãy test trên postman

        - User bị Inactive thì thể cập nhật gadget được.

        - GadgetDescriptions:
            - **image** và **text** có thể null
            - **type** có thể là: image, normalText, boldText
            - nếu **type** là image thì field **image** phải chứa file hình ảnh
            - nếu **type** không phải là image thì field **text** phải chứa text tương ứng
            - thứ tự của array sẽ được dùng để tạo index tương ứng, hãy lưu ý

        - ThumbnailUrl:
            - nếu update thì thêm file vào field ThumbnailUrl
            - nếu giữ nguyên thì ThumbnailUrl để null

        - GadgetImages:
            - nếu tạo mới thì id để null, gắn file vào field image
            - nếu xoá thì không cần thêm item vào array
        
        - GadgetDescriptions:
            - nếu tạo mới thì id để null
            - nếu tạo mới image thì gắn file vào field image
            - nếu sửa thì gắn kém id cũ và nội dung cần chỉnh sửa

        - SpecificationValues:
            - nếu update item thì truyền id cũ 
            - nếu xoá item thì không cần truyền item vào 
            - nếu tạo mới item thì id để null
        """
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromForm] Request request, Guid id, AppDbContext context, CurrentUserService currentUserService,
                                               GoogleStorageService storageService, EmbeddingService embeddingService)
    {
        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Status == UserStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("user", "Tài khoản của bạn đã bị khóa, không thể thực hiện thao tác này.")
            .Build();
        }

        var gadget = await context.Gadgets
                                .Include(g => g.GadgetImages)
                                .Include(g => g.GadgetDescriptions)
                                .Include(g => g.SpecificationValues)
                                .FirstOrDefaultAsync(g => g.Id == id);
        if (gadget is null)
        {
            throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadget", "Sản phẩm không tồn tại")
                        .Build();
        }

        if (gadget.SellerId != currentUser!.Seller!.Id)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("role", "Tài khoản không đủ thẩm quyền để thực hiện hành động này.")
                .Build();
        }

        if (gadget.Status == GadgetStatus.Inactive)
        {
            throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEA_00)
                .AddReason("role", "Sản phẩm đã bị khoá, bạn không thể thực hiện hành động này")
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

            if (specValue.Id.HasValue)
            {
                var specValueExists = await context.SpecificationValues
                                    .AnyAsync(u => u.Id == specValue.Id);

                if (!specValueExists)
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("specificationValue", $"specificationValue {specValue.Id} không tồn tại")
                        .Build();
                }
            }
        }

        string? thumbnailUrl = null;
        if (request.ThumbnailUrl != null)
        {
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
        }

        List<GadgetImage> gadgetImages = [];

        foreach (var item in request.GadgetImages)
        {
            if (item.Id == null)
            {
                var url = await storageService.UploadFileToCloudStorage(item.Image!, Guid.NewGuid().ToString());
                gadgetImages.Add(new GadgetImage
                {
                    ImageUrl = url
                });
            }
            else
            {
                var existItem = await context.GadgetImages.FirstOrDefaultAsync(gi => gi.Id == item.Id.Value);
                if (existItem == null)
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadgetImage", $"gadgetImage {item.Id} không tồn tại")
                        .Build();
                }

                if (item.Image != null)
                {
                    string? url = null;
                    try
                    {
                        url = await storageService.UploadFileToCloudStorage(item.Image, Guid.NewGuid().ToString());
                    }
                    catch (Exception)
                    {
                        if (url != null)
                        {
                            await storageService.DeleteFileFromCloudStorage(url);
                        }
                        throw TechGadgetException.NewBuilder()
                            .WithCode(TechGadgetErrorCode.WES_00)
                            .AddReason("gadgetImage", "Lỗi khi lưu hình ảnh Sản phẩm")
                            .Build();
                    }
                    existItem.ImageUrl = url;
                }

                gadgetImages.Add(existItem);
            }
        }



        List<GadgetDescription> gadgetDescriptions = [];
        int index = 0;

        foreach (var gadgetDescriptionRequest in request.GadgetDescriptions)
        {
            if (gadgetDescriptionRequest.Id == null)
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
            else
            {
                var existDescription = await context.GadgetDescriptions.FirstOrDefaultAsync(gi => gi.Id == gadgetDescriptionRequest.Id);
                if (existDescription == null)
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("gadgetDescription", $"gadgetDescritption {gadgetDescriptionRequest.Id} không tồn tại")
                        .Build();
                }

                existDescription.Index = index++;

                if (gadgetDescriptionRequest.Type != GadgetDescriptionType.Image)
                {
                    if (gadgetDescriptionRequest.Text == null)
                    {
                        throw TechGadgetException.NewBuilder()
                            .WithCode(TechGadgetErrorCode.WES_00)
                            .AddReason("gadgetDescription", $"gadgetDescritption {gadgetDescriptionRequest.Id} không hợp lệ: field text không được để trống khi type khác image")
                            .Build();
                    }

                    existDescription.Value = gadgetDescriptionRequest.Text;
                }

                if (gadgetDescriptionRequest.Type == GadgetDescriptionType.Image && gadgetDescriptionRequest.Image != null)
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
                    existDescription.Value = url;
                }

                existDescription.Type = gadgetDescriptionRequest.Type;

                gadgetDescriptions.Add(existDescription);
            }
        }

        List<SpecificationValue> specificationValues = [];
        foreach (var s in request.SpecificationValues)
        {
            if (s.Id != null)
            {
                var existSpecValue = await context.SpecificationValues.FirstOrDefaultAsync(sv => sv.Id == s.Id);
                if (existSpecValue == null)
                {
                    throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_00)
                        .AddReason("specificationValue", $"specificationValue {s.Id} không tồn tại")
                        .Build();
                }
                existSpecValue!.SpecificationUnitId = s.SpecificationUnitId;
                existSpecValue.SpecificationKeyId = s.SpecificationKeyId;
                existSpecValue.Value = s.Value;
                existSpecValue.Vector = await embeddingService.GetEmbedding(s.Value);
                specificationValues.Add(existSpecValue);
            }
            else
            {
                specificationValues.Add(new SpecificationValue
                {
                    SpecificationKeyId = s.SpecificationKeyId,
                    SpecificationUnitId = s.SpecificationUnitId,
                    Value = s.Value,
                    Vector = await embeddingService.GetEmbedding(s.Value)
                });
            }
        }

        gadget.BrandId = request.BrandId;
        gadget.Name = request.Name;
        gadget.Price = request.Price;
        if (thumbnailUrl != null)
        {
            gadget.ThumbnailUrl = thumbnailUrl;
        }
        gadget.CategoryId = request.CategoryId;
        gadget.UpdatedAt = DateTime.UtcNow;
        gadget.Condition = request.Condition;
        gadget.ConditionVector = await embeddingService.GetEmbedding(request.Condition);
        gadget.NameVector = await embeddingService.GetEmbedding(request.Name);
        gadget.Quantity = request.Quantity;
        gadget.GadgetImages = gadgetImages;
        gadget.GadgetDescriptions = gadgetDescriptions;
        gadget.SpecificationValues = specificationValues;

        await context.SaveChangesAsync();

        return Ok("Cập nhật thành công");
    }
}
