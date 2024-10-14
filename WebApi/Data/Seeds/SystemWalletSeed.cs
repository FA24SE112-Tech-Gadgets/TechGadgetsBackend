using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SystemWalletSeed
{
    public readonly static List<SystemWallet> Default =
    [
        new SystemWallet { Id = Guid.Parse("e2bc27f5-e0ce-48d6-9829-943e1d657e81"), Amount = 0 }
    ];
}
