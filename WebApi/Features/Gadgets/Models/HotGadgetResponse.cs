﻿using WebApi.Data.Entities;

namespace WebApi.Features.Gadgets.Models;

public class HotGadgetResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public string ThumbnailUrl { get; set; } = default!;
    public UserStatus SellerStatus { get; set; }
}
