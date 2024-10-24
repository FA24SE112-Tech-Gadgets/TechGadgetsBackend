﻿namespace WebApi.Features.OrderDetails.Models;

public class GadgetInformationOrderDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Price { get; set; } = default!;
    public string ThumbnailUrl { get; set; } = default!;
    public int Quantity { get; set; } = default!;
}
