namespace WebApi.Features.Payments.Models;

public class MomoWebhookRequest
{
    public string PartnerCode { get; set; } = default!;

    public string OrderId { get; set; } = default!;

    public string RequestId { get; set; } = default!;

    public decimal Amount { get; set; }

    public string OrderInfo { get; set; } = default!;

    public string OrderType { get; set; } = default!;

    public long TransId { get; set; } = default!;

    public int ResultCode { get; set; }

    public string Message { get; set; } = default!;

    //[FromQuery(Name = "payType")]
    //public string PayType { get; set; } = default!;

    public long ResponseTime { get; set; } = default!;

    public string ExtraData { get; set; } = default!;

    public string Signature { get; set; } = default!;

    public bool IsSuccess => ResultCode == 0;

    public string ReturnUrl { get; set; } = default!;
}
