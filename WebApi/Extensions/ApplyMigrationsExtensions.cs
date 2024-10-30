using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Entities;
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

        if (!await context.CustomerInformation.AnyAsync())
        {
            foreach (var customerInformation in CustomerInformationSeed.Default)
            {
                context.CustomerInformation.Add(customerInformation);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.SellerInformation.AnyAsync())
        {
            foreach (var sellerInformation in SellerInformationSeed.Default)
            {
                context.SellerInformation.Add(sellerInformation);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.SellerApplications.AnyAsync())
        {
            foreach (var application in SellerApplicationSeed.Default)
            {
                application.CreatedAt = DateTime.UtcNow;
                context.SellerApplications.Add(application);
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

        if (!await context.SpecificationKeys.AnyAsync())
        {
            foreach (var specificationKey in SpecificationKeySeed.Default)
            {
                context.SpecificationKeys.Add(specificationKey);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.SpecificationUnits.AnyAsync())
        {
            foreach (var specificationUnit in SpecificationUnitSeed.Default)
            {
                context.SpecificationUnits.Add(specificationUnit);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.GadgetFilters.AnyAsync())
        {
            foreach (var gadgetFilter in GadgetFilterSeed.Default)
            {
                gadgetFilter.Vector = await embeddingService.GetEmbedding(gadgetFilter.Value);
                context.GadgetFilters.Add(gadgetFilter);
            }
            await context.SaveChangesAsync();
        }

        if (!await context.GadgetDiscounts.AnyAsync() && await context.Gadgets.AnyAsync())
        {
            foreach (var gadgetDiscount in GadgetDiscountSeed.Default)
            {
                gadgetDiscount.ExpiredDate = DateTime.UtcNow.AddMonths(6);
                gadgetDiscount.Status = GadgetDiscountStatus.Active;
                gadgetDiscount.CreatedAt = DateTime.UtcNow;

                context.GadgetDiscounts.Add(gadgetDiscount);
            }
            await context.SaveChangesAsync();
        }

        //if (!await context.Gadgets.AnyAsync())
        //{
        //    // Create lists to hold the names and conditions
        //    var names = GadgetSeed.Default.Select(gadget => gadget.Name).ToList();
        //    var conditions = GadgetSeed.Default.Select(gadget => gadget.Condition).ToList();

        //    // Get embeddings for names and conditions in a batch
        //    var nameVectors = await embeddingService.GetEmbeddings(names);
        //    var conditionVectors = await embeddingService.GetEmbeddings(conditions);

        //    var now = DateTime.UtcNow;
        //    int index = 0;
        //    foreach (var gadget in GadgetSeed.Default)
        //    {
        //        gadget.NameVector = nameVectors[index];
        //        gadget.ConditionVector = conditionVectors[index];
        //        gadget.Status = GadgetStatus.Active;
        //        gadget.IsForSale = true;
        //        gadget.CreatedAt = now;
        //        gadget.UpdatedAt = now;
        //        gadget.Quantity = 50;

        //        context.Gadgets.Add(gadget);
        //        index++;
        //    }


        //    foreach (var gadgetImage in GadgetImageSeed.Default)
        //    {
        //        context.GadgetImages.Add(gadgetImage);
        //    }

        //    foreach (var gadgetDescription in GadgetDescriptionSeed.Default)
        //    {
        //        context.GadgetDescriptions.Add(gadgetDescription);
        //    }


        //    var values = SpecificationValueLaptopSeed.Default.Select(s => s.Value).ToList();
        //    var valueVectors = await embeddingService.GetEmbeddings(values);
        //    index = 0;
        //    foreach (var specificationValue in SpecificationValueLaptopSeed.Default)
        //    {
        //        specificationValue.Vector = valueVectors[index];

        //        context.SpecificationValues.Add(specificationValue);
        //        index++;
        //    }

        //    values = SpecificationValueDienThoaiSeed.Default.Select(s => s.Value).ToList();
        //    valueVectors = await embeddingService.GetEmbeddings(values);
        //    index = 0;
        //    foreach (var specificationValue in SpecificationValueDienThoaiSeed.Default)
        //    {
        //        specificationValue.Vector = valueVectors[index];

        //        context.SpecificationValues.Add(specificationValue);
        //        index++;
        //    }

        //    values = SpecificationValueTaiNgheSeed.Default.Select(s => s.Value).ToList();
        //    valueVectors = await embeddingService.GetEmbeddings(values);
        //    index = 0;
        //    foreach (var specificationValue in SpecificationValueTaiNgheSeed.Default)
        //    {
        //        specificationValue.Vector = valueVectors[index];

        //        context.SpecificationValues.Add(specificationValue);
        //        index++;
        //    }

        //    values = SpecificationValueLoaSeed.Default.Select(s => s.Value).ToList();
        //    valueVectors = await embeddingService.GetEmbeddings(values);
        //    index = 0;
        //    foreach (var specificationValue in SpecificationValueLoaSeed.Default)
        //    {
        //        specificationValue.Vector = valueVectors[index];

        //        context.SpecificationValues.Add(specificationValue);
        //        index++;
        //    }

        //    await context.SaveChangesAsync();
        //}
    }
}
