using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Common.Exceptions;
using WebApi.Common.Filters;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Features.Orders.Mappers;
using WebApi.Services.Auth;

namespace WebApi.Features.Orders;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
[RequestValidation<Request>]
public class CreateOrderNow : ControllerBase
{
    public new class Request
    {
        public Guid GadgetId { get; set; } = default!;
        public int Quantity { get; set; } = 1;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.GadgetId)
                .NotEmpty()
                .WithMessage("Sản phẩm không được để trống");
        }
    }

    [HttpPost("order/now")]
    [Tags("Orders")]
    [SwaggerOperation(
        Summary = "Create Order Now",
        Description = "This API is for customer buy now. Note: " +
                            "<br>&nbsp; - Dùng API này để mua ngay sản phẩm mà không cần add to cart(trừ tiền có sẵn trong ví)." +
                            "<br>&nbsp; - API này không tác động gì đến cart hết." +
                            "<br>&nbsp; - Tạo đơn thanh toán cho chúng. Cũng như là trừ tiền trong ví" +
                            "<br>&nbsp; - Customer cần điền Address và PhoneNumber trước khi tiến hành tạo order (Trước khi gọi API)" +
                            "<br>&nbsp; - Default quantity = 1 nếu không truyền."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
        var gadgetItem = await context.Gadgets
            .Include(g => g.GadgetDiscounts)
            .FirstOrDefaultAsync(g => g.Id == request.GadgetId);

        if (gadgetItem == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", "Sản phẩm không tồn tại.")
            .Build();
        }

        var currentUser = await currentUserService.GetCurrentUser();

        if (currentUser!.Customer!.Address == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("customers", "Người dùng chưa nhập địa chỉ nhận hàng.")
            .Build();
        }

        if (currentUser!.Customer!.PhoneNumber == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("customers", "Người dùng chưa nhập số điện thoại nhận hàng.")
            .Build();
        }

        // Truy vấn để lấy seller từ giỏ hàng của user
        var seller = await context.Gadgets
            .Where(g => g.Id == request.GadgetId)
            .Include(g => g.Seller)
                .ThenInclude(s => s.User)
            .Select(g => g.Seller)
            .FirstOrDefaultAsync();

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);
        var systemWallet = await context.SystemWallets.SingleOrDefaultAsync();

        int totalAmount = 0;

        Order order = new Order()
        {
            CustomerId = currentUser!.Customer!.Id,
        }!;

        List<SellerOrder> sellerOrders = new List<SellerOrder>()!;

        //Validate user(seller) status inactive
        if (seller!.User.Status != UserStatus.Active)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_02)
            .AddReason("seller", $"Người bán {seller.Id} đã bị vô hiệu hóa.")
            .Build();
        }

        var customerInformation = await context.CustomerInformation
            .OrderByDescending(ci => ci.CreatedAt)
            .FirstOrDefaultAsync(ci => ci.CustomerId == currentUser!.Customer!.Id);

        var sellerInformation = await context.SellerInformation
            .OrderByDescending(ci => ci.CreatedAt)
            .FirstOrDefaultAsync(ci => ci.SellerId == seller.Id);

        var createdAt = DateTime.UtcNow;
        SellerOrder sellerOrder = new SellerOrder()
        {
            SellerId = seller.Id,
            CustomerInformation = customerInformation!,
            SellerInformation = sellerInformation!,
            Status = SellerOrderStatus.Pending,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
        }!;
        List<SellerOrderItem> sellerOrderItems = new List<SellerOrderItem>()!;

        if (gadgetItem!.SellerId == seller.Id)
        {
            SellerOrderItem sellerOrderItem = gadgetItem.ToGadgetInformation()!;

            //Validate gadget status inactive
            if (gadgetItem.Status != GadgetStatus.Active)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", $"Sản phẩm {request.GadgetId} đã bị vô hiệu hóa.")
                .Build();
            }

            //Validate gadget IsForSale
            if (!gadgetItem.IsForSale)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", $"Sản phẩm {request.GadgetId} đã ngừng kinh doanh.")
                .Build();
            }

            //Validate gadget không còn đủ sản phẩm
            if (gadgetItem.Quantity < request.Quantity)
            {
                throw TechGadgetException.NewBuilder()
                .WithCode(TechGadgetErrorCode.WEB_02)
                .AddReason("gadget", $"Số lượng sản phẩm {request.GadgetId} không đủ.")
                .Build();
            }
            gadgetItem.Quantity -= request.Quantity;

            sellerOrderItem.GadgetQuantity = request.Quantity;
            sellerOrderItems.Add(sellerOrderItem);

            //Tính tổng giá tiền order
            int discountPercentage = gadgetItem.GadgetDiscounts
                .FirstOrDefault(gd => gd.Status == GadgetDiscountStatus.Active)?.DiscountPercentage ?? 0;
            totalAmount += (int)Math.Ceiling(request.Quantity * gadgetItem.Price * (1 - discountPercentage / 100.0));
        }

        sellerOrder.SellerOrderItems = sellerOrderItems;
        sellerOrders.Add(sellerOrder);

        // Tạo systemOrderDetailTracking để tracking orderDetail mới tạo
        createdAt = DateTime.UtcNow;
        SystemSellerOrderTracking systemOrderDetailTracking = new SystemSellerOrderTracking()
        {
            SystemWalletId = systemWallet!.Id,
            SellerOrder = sellerOrder,
            FromUserId = currentUser.Id,
            ToUserId = seller.UserId,
            Status = SystemSellerOrderTrackingStatus.Pending,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
        }!;
        await context.SystemSellerOrderTrackings.AddAsync(systemOrderDetailTracking);

        order.SellerOrders = sellerOrders;

        //Check số dư ví coi đủ để thanh toán không
        if (userWallet!.Amount == 0 || totalAmount > userWallet!.Amount)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_03)
            .AddReason("wallets", "Số dư trong ví không đủ.")
            .Build();
        }

        //Thanh toán, trừ tiền userWallet
        userWallet.Amount -= totalAmount;

        //Cộng tiền đó vô systemWallet
        systemWallet!.Amount += totalAmount;

        //Tạo lịch sử WalletTracking cho việc trừ tiền đó
        WalletTracking walletTracking = new WalletTracking()
        {
            WalletId = userWallet!.Id,
            Order = order,
            Amount = totalAmount,
            Type = WalletTrackingType.Payment,
            Status = WalletTrackingStatus.Success,
            CreatedAt = DateTime.UtcNow,
        }!;

        await context.WalletTrackings.AddAsync(walletTracking);

        //Save tất cả mọi thứ vô DB
        if ((sellerOrders.Count > 0 && totalAmount > 0) || request.Quantity > 0)
        {
            await context.SaveChangesAsync();
        }
        else
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("error", "Có lỗi xảy ra trong quá trình tạo đơn.")
            .Build();
        }

        return Ok();
    }
}
