using WebApi.Data.Entities;
using WebApi.Features.Users.Models;

namespace WebApi.Features.Users.Mappers;

public static class WalletMapper
{
    public static WalletResponse? ToWalletResponse(this Wallet? wallet)
    {
        if (wallet != null)
        {
            return new WalletResponse
            {
                Id = wallet.Id,
                Amount = wallet.Amount,
            };
        }
        return null;
    }
}
