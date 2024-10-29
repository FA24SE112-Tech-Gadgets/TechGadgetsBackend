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
public class CreateOrder : ControllerBase
{
    public new class Request
    {
        public List<Guid> ListGadgetItems { get; set; } = default!;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(sp => sp.ListGadgetItems)
                .NotEmpty()
                .WithMessage("Danh sách Gadget không được để trống")
                .Must(list => list.Count > 0)
                .WithMessage("Danh sách Gadget phải có ít nhất một phần tử.");
        }
    }

    [HttpPost("order")]
    [Tags("Orders")]
    [SwaggerOperation(
        Summary = "Create Order",
        Description = "This API is for customer create order. Note: " +
                            "<br>&nbsp; - Dùng API này để thanh toán đơn hàng(trừ tiền có sẵn trong ví)." +
                            "<br>&nbsp; - Sau khi gọi API này thì những gadget thanh toán, sẽ không còn nằm trong cart nữa." +
                            "<br>&nbsp; - Đồng thời tạo đơn thanh toán cho chúng. Cũng như là trừ tiền trong ví" +
                            "<br>&nbsp; - Customer cần điền Address trước khi tiến hành tạo order (Trước khi gọi API)" +
                            "<br>&nbsp; - Không thể tạo đơn với những sản phẩm nằm ngoài giỏ hàng."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TechGadgetErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Handler([FromBody] Request request, AppDbContext context, [FromServices] CurrentUserService currentUserService)
    {
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

        var userCart = await context.Carts
            .FirstOrDefaultAsync(c => c.CustomerId == currentUser!.Customer!.Id);

        if (userCart == null)
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("carts", "Không tìm thấy giỏ hàng.")
            .Build();
        }

        // Truy vấn để lấy seller từ giỏ hàng của user
        var sellers = await context.CartGadgets
            .Include(cg => cg.Gadget)
                .ThenInclude(g => g.Seller)
                .ThenInclude(s => s.User)
            .Where(cg => request.ListGadgetItems.Contains(cg.GadgetId) && cg.CartId == userCart.Id)
            .Select(cg => cg.Gadget.Seller)
            .Distinct()
            .ToListAsync();

        // Lấy list cartGadget của user
        var listCartGadgets = await context.CartGadgets
            .Include(cg => cg.Gadget)
            .Where(cg => request.ListGadgetItems.Contains(cg.GadgetId) && cg.CartId == userCart.Id)
            .ToListAsync();

        var userWallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == currentUser!.Id);
        var systemWallet = await context.SystemWallets.SingleOrDefaultAsync();

        int totalAmount = 0;

        Order order = new Order()
        {
            CustomerId = currentUser!.Customer!.Id,
        }!;

        List<SellerOrder> sellerOrders = new List<SellerOrder>()!;

        //Chia order theo từng seller
        foreach (var seller in sellers)
        {
            //Validate user(seller) status inactive
            if (seller.User.Status != UserStatus.Active)
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
            foreach (var cartGadget in listCartGadgets)
            {
                if (cartGadget.Gadget.SellerId == seller.Id)
                {
                    SellerOrderItem sellerOrderItem = cartGadget.Gadget.ToGadgetInformation()!;

                    //Validate gadget status inactive
                    if (cartGadget.Gadget.Status != GadgetStatus.Active)
                    {
                        throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", $"Sản phẩm {cartGadget.GadgetId} đã bị vô hiệu hóa.")
                        .Build();
                    }

                    //Validate gadget IsForSale
                    if (!cartGadget.Gadget.IsForSale)
                    {
                        throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", $"Sản phẩm {cartGadget.GadgetId} đã ngừng kinh doanh.")
                        .Build();
                    }

                    //Validate gadget không còn đủ sản phẩm
                    if (cartGadget.Gadget.Quantity < cartGadget.Quantity)
                    {
                        throw TechGadgetException.NewBuilder()
                        .WithCode(TechGadgetErrorCode.WEB_02)
                        .AddReason("gadget", $"Số lượng sản phẩm {cartGadget.GadgetId} không đủ.")
                        .Build();
                    }
                    cartGadget.Gadget.Quantity -= cartGadget.Quantity;

                    sellerOrderItem.GadgetQuantity = cartGadget.Quantity;
                    sellerOrderItems.Add(sellerOrderItem);

                    //Tính tổng giá tiền order
                    totalAmount += (cartGadget.Quantity * cartGadget.Gadget.Price);

                    //Xóa gadget ra khỏi cart
                    context.CartGadgets.Remove(cartGadget);
                }
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
        }
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
        if ((sellerOrders.Count > 0 && totalAmount > 0) || listCartGadgets.Count > 0)
        {
            await context.SaveChangesAsync();
        } else
        {
            throw TechGadgetException.NewBuilder()
            .WithCode(TechGadgetErrorCode.WEB_00)
            .AddReason("gadget", "Sản phẩm không có trong giỏ hàng.")
            .Build();
        }

        return Ok();
    }
}
