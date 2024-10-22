﻿namespace WebApi.Features.OrderDetails.Models;

public class CustomerInfoResponse
{
    public string FullName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}