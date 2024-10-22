namespace WebApi.Features.Payments.Models;

public class VnPayWebhookRequest
{
    public string? Vnp_TransactionStatus { get; set; } = default!;

    public string? Vnp_TmnCode { get; set; } = default!;
    public long? Vnp_Amount { get; set; } = default!;

    public string? Vnp_BankCode { get; set; } = default!;

    public string? Vnp_BankTranNo { get; set; } = default!;

    public string? Vnp_CardType { get; set; } = default!;

    public string? Vnp_PayDate { get; set; } = default!;

    public string? Vnp_SecureHashType { get; set; } = default!;

    public string? Vnp_TransactionNo { get; set; } = default!;

    public string? Vnp_TxnRef { get; set; } = default!;

    public string? Vnp_OrderInfo { get; set; } = default!;

    public string? Vnp_SecureHash { get; set; } = default!;

    public string? Vnp_ResponseCode { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;

    public bool IsSuccess => "00".Equals(Vnp_ResponseCode);
}
