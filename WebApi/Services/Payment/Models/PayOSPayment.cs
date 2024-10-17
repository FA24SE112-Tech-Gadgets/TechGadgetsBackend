﻿namespace WebApi.Services.Payment.Models;

public class PayOSPayment
{
    public string PaymentReferenceId { get; set; } = default!;

    public int Amount { get; set; }

    public string? Info { get; set; }

    public string ReturnUrl { get; set; } = default!;
}
