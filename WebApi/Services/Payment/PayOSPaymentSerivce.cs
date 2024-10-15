using Microsoft.Extensions.Options;
using Net.payOS.Types;
using Net.payOS;
using WebApi.Common.Settings;
using WebApi.Services.Server;
using WebApi.Services.Payment.Models;

namespace WebApi.Services.Payment;

public class PayOSPaymentSerivce(IOptions<PayOSSettings> payOSSettings, CurrentServerService currentServerService)
{
    private const string DefaultOrderInfo = "Thanh toán với PayOS";

    private readonly PayOSSettings _payOSSettings = payOSSettings.Value;

    public async Task<string> CreatePaymentAsync(PayOSPayment payment)
    {
        //Create list item for payment link
        List<ItemData> items = new List<ItemData>();
        items.Add(new ItemData("Thanh toán ví điện tử TechGadget", 1, payment.Amount));

        var request = new PayOSPaymentRequest
        {
            OrderCode = GenerateDailyRandomLong(),
            Amount = payment.Amount,
            Description = payment.Info ?? DefaultOrderInfo,
            Items = items,
            CancelUrl = payment.ReturnUrl,
            ReturnUrl = payment.ReturnUrl,
        };

        PaymentData paymentData = new PaymentData(request.OrderCode, request.Amount, request.Description, request.Items, request.CancelUrl, request.ReturnUrl, null, null, null, null, null, GetExpireAt())!;

        PayOS payOS = new PayOS(_payOSSettings.ClientID, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey)!;

        var createPaymentResult = await payOS.createPaymentLink(paymentData);
        
        return createPaymentResult.checkoutUrl;
    }

    private static int GetExpireAt()
    {
        // Lấy thời gian hiện tại
        DateTime currentTime = DateTime.UtcNow;

        // Thêm 5 phút vào thời gian hiện tại
        DateTime expirationTime = currentTime.AddMinutes(5);

        // Chuyển đổi thời gian hết hạn thành Unix Timestamp
        int unixTimestamp = (int)(expirationTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp;
    }

    private long GenerateDailyRandomLong()
    {
        // Lấy ngày hiện tại (chỉ lấy phần ngày)
        DateTime today = DateTime.Today;

        // Tạo một seed từ ngày hôm nay bằng cách sử dụng tổng các thành phần ngày
        int seed = today.Year * 10000 + today.Month * 100 + today.Day;

        // Sử dụng seed để tạo Random (mỗi ngày sẽ có seed khác nhau)
        Random random = new Random(seed);

        // Tạo số long ngẫu nhiên từ Random
        byte[] buffer = new byte[8];
        random.NextBytes(buffer);

        // Chuyển đổi thành long
        long randomLong = BitConverter.ToInt64(buffer, 0);

        // Đảm bảo giá trị dương (nếu cần)
        randomLong = Math.Abs(randomLong);

        return randomLong;
    }
}
