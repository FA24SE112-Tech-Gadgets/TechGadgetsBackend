﻿namespace WebApi.Data.Entities;

public class SystemWallet
{
    public Guid Id { get; set; }
    public long Amount { get; set; }

    public ICollection<SystemSellerOrderTracking> SystemOrderDetailTrackings { get; set; } = [];
}
