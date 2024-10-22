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
        var sellers = await context.Carts
            .Where(c => c.CustomerId == currentUser!.Customer!.Id)   // Lọc theo giỏ hàng của user
            .SelectMany(c => c.CartGadgets.Select(cg => cg.Gadget.Seller)) // Lấy seller từ các sản phẩm trong giỏ hàng
            .Distinct()  // Loại bỏ seller trùng lặp
            .Include(s => s.User)
            .OrderBy(s => s.Id) // Sắp xếp để có thể phân trang
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

        List<OrderDetail> orderDetails = new List<OrderDetail>()!;

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

            OrderDetail orderDetail = new OrderDetail()
            {
                SellerId = seller.Id,
                Status = OrderDetailStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            }!;
            List<GadgetInformation> gadgetInformations = new List<GadgetInformation>()!;
            int orderDetailAmount = 0;
            foreach (var cartGadget in listCartGadgets)
            {
                if (cartGadget.Gadget.SellerId == seller.Id)
                {
                    GadgetInformation gadgetInformation = cartGadget.Gadget.ToGadgetInformation()!;

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

                    gadgetInformation.GadgetQuantity = cartGadget.Quantity;
                    gadgetInformations.Add(gadgetInformation);

                    //Tính tổng giá tiền orderDetail
                    //orderDetailAmount += (cartGadget.Quantity * cartGadget.Gadget.Price);
                    orderDetailAmount += 2000;

                    //Tính tổng giá tiền order
                    //totalAmount += (cartGadget.Quantity * cartGadget.Gadget.Price);
                    totalAmount += 2000;

                    //Xóa gadget ra khỏi cart
                    context.CartGadgets.Remove(cartGadget);
                }
            }
            orderDetail.Amount = orderDetailAmount;
            orderDetail.GadgetInformation = gadgetInformations;
            orderDetails.Add(orderDetail);

            //Tạo customerInfo để lưu cứng
            CustomerInformation customerInformation = new CustomerInformation()
            {
                CustomerId = currentUser.Customer.Id,
                FullName = currentUser.Customer.FullName,
                Address = currentUser.Customer.Address,
                PhoneNumber = currentUser.Customer.PhoneNumber!,
                OrderDetail = orderDetail,
            }!;
            await context.CustomerInformation.AddAsync(customerInformation);

            //Tạo sellerInfo để lưu cứng
            SellerInformation sellerInformation = new SellerInformation()
            {
                SellerId = seller.Id,
                ShopName = seller.ShopName,
                PhoneNumber = seller.PhoneNumber,
                Address = seller.ShopAddress,
                OrderDetail = orderDetail,
            }!;
            await context.SellerInformation.AddAsync(sellerInformation);

            // Tạo systemOrderDetailTracking để tracking orderDetail mới tạo
            SystemOrderDetailTracking systemOrderDetailTracking = new SystemOrderDetailTracking()
            {
                SystemWalletId = systemWallet!.Id,
                OrderDetail = orderDetail,
                FromUserId = currentUser.Id,
                ToUserId = seller.UserId,
                Status = SystemOrderDetailTrackingStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            }!;
            await context.SystemOrderDetailTrackings.AddAsync(systemOrderDetailTracking);
        }
        order.OrderDetails = orderDetails;

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
        if ((orderDetails.Count >= 0 && totalAmount >= 0) || listCartGadgets.Count >= 0)
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
