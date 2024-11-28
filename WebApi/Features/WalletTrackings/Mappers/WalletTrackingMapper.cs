using WebApi.Data.Entities;
using WebApi.Features.WalletTrackings.Models;

namespace WebApi.Features.WalletTrackings.Mappers;

public static class WalletTrackingMapper
{
    public static WalletTrackingItemResponse? ToWalletTrackingItemResponse(this WalletTracking walletTracking)
    {
        if (walletTracking != null)
        {
            return new WalletTrackingItemResponse
            {
                Id = walletTracking.Id,
                PaymentMethod = walletTracking.PaymentMethod,
                Amount = walletTracking.Amount,
                BalanceBeforeChange = walletTracking.BalanceBeforeChange,
                Type = walletTracking.Type,
                Status = walletTracking.Status,
                OrderId = walletTracking.OrderId,
                SellerOrderId = walletTracking.SellerOrderId,
                RefundedAt = walletTracking.RefundedAt,
                DepositedAt = walletTracking.DepositedAt,
                CreatedAt = walletTracking.CreatedAt,
            };
        }
        return null;
    }
}
