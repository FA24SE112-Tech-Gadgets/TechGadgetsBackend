using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Seeds;
using WebApi.Services.Embedding;

namespace WebApi.Extensions;

public static class ApplyMigrationsExtensions
{
    public static async void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        var embeddingService = services.GetRequiredService<EmbeddingService>();

        if (!await context.Users.AnyAsync())
        {
            foreach (var user in UserSeed.Default)
            {
                if (user.Seller != null)
                {
                    user.Seller.AddressVector = await embeddingService.GetEmbedding(user.Seller.ShopAddress);
                }
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.Brands.AnyAsync())
        {
            foreach (var brand in BrandSeed.Default)
            {
                context.Brands.Add(brand);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.Categories.AnyAsync())
        {
            foreach (var category in CategorySeed.Default)
            {
                context.Categories.Add(category);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.CategoryBrands.AnyAsync())
        {
            foreach (var categoryBrand in CategoryBrandSeed.Default)
            {
                context.CategoryBrands.Add(categoryBrand);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.SystemWallets.AnyAsync())
        {
            foreach (var systemWallet in SystemWalletSeed.Default)
            {
                context.SystemWallets.Add(systemWallet);
            }
            await context.SaveChangesAsync();
        }
    }
}
