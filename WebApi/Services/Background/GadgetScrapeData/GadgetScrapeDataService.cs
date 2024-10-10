using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Embedding;
using WebApi.Services.ScrapeData;

namespace WebApi.Services.Background.GadgetScrapeData;

public class GadgetScrapeDataService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var scrapeTGDDDataService = scope.ServiceProvider.GetRequiredService<ScrapeTGDDDataService>();
                var scrapeFPTShopDataService = scope.ServiceProvider.GetRequiredService<ScrapeFPTShopDataService>();
                var scrapePhongVuDataService = scope.ServiceProvider.GetRequiredService<ScrapePhongVuDataService>();
                //await scrapeTGDDDataService.ScrapeTGDDGadget();
                //await scrapeFPTShopDataService.ScrapeFPTShopGadget();
                await scrapePhongVuDataService.ScrapeGadgetByBrand("https://phongvu.vn/c/phone-dien-thoai?brands=samsung");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    public async Task AddGadgetToDB(List<Gadget> gadgets, Brand relatedBrand, Category relatedCategory, Shop shop, AppDbContext context, EmbeddingService embeddingService)
    {
        foreach (var gadget in gadgets)
        {
            var existGadget = await context.Gadgets
                .Include(g => g.Specifications)
                .ThenInclude(s => s.SpecificationKeys)
                .ThenInclude(sk => sk.SpecificationValues)
                .FirstOrDefaultAsync(g => (g.Name == gadget.Name && g.ShopId == shop.Id));
            if (existGadget != null)
            {
                // Lặp qua các Specifications của Gadget
                foreach (var specification in existGadget.Specifications)
                {
                    // Lặp qua các SpecificationKeys của mỗi Specification
                    foreach (var specificationKey in specification.SpecificationKeys)
                    {
                        // Xóa tất cả SpecificationValues liên quan đến SpecificationKey
                        context.SpecificationValues.RemoveRange(specificationKey.SpecificationValues);
                    }

                    // Xóa tất cả SpecificationKeys liên quan đến Specification
                    context.SpecificationKeys.RemoveRange(specification.SpecificationKeys);
                }

                // Xóa tất cả Specifications liên quan đến Gadget
                context.Specifications.RemoveRange(existGadget.Specifications);

                // Lặp qua các SpecificationKey của Gadget
                foreach (var specificationKey in existGadget.SpecificationKeys)
                {
                    // Xóa tất cả SpecificationValues liên quan đến SpecificationKey
                    context.SpecificationValues.RemoveRange(specificationKey.SpecificationValues);
                }
                
                // Xóa tất cả SpecificationKeys liên quan đến Gadget
                context.SpecificationKeys.RemoveRange(existGadget.SpecificationKeys);

                // Xóa tất cả GadgetDescriptions liên quan đến Gadget
                context.GadgetDescriptions.RemoveRange(existGadget.GadgetDescriptions);

                // Xóa tất cả GadgetImages liên quan đến Gadget
                context.GadgetImages.RemoveRange(existGadget.GadgetImages);

                existGadget.Price = gadget.Price;

                if (gadget.ThumbnailUrl != null)
                {
                    existGadget.ThumbnailUrl = gadget.ThumbnailUrl;
                }

                if (gadget.Url != null)
                {
                    existGadget.Url = gadget.Url;
                }

                if (gadget.Specifications != null)
                {
                    existGadget.Specifications = gadget.Specifications;
                    context.Specifications.AddRange(existGadget.Specifications);
                }

                if (gadget.SpecificationKeys != null)
                {
                    existGadget.SpecificationKeys = gadget.SpecificationKeys;
                    context.SpecificationKeys.AddRange(existGadget.SpecificationKeys);
                }

                existGadget.UpdatedAt = DateTime.UtcNow;

                if (existGadget.Specifications != null && existGadget.Specifications.Count > 0)
                {
                    string keys = "";
                    foreach (var spe in existGadget.Specifications)
                    {
                        var gadgetSpecificationKeys = spe.SpecificationKeys;
                        foreach (var key in gadgetSpecificationKeys)
                        {
                            string values = "";
                            foreach (var value in key.SpecificationValues)
                            {
                                if (!value.Value.IsNullOrEmpty())
                                {
                                    values += (" " + value.Value.Trim().ToLower());
                                }
                            }
                            if (!key.Name.IsNullOrEmpty())
                            {
                                keys += (" " + key.Name.Trim().ToLower() + values);
                            }
                        }
                    }
                    if (!keys.IsNullOrEmpty())
                    {
                        var gadgetVector = await embeddingService.GetEmbedding(existGadget.Name.Trim().ToLower() + " " + keys.Trim());
                        existGadget.Vector = gadgetVector;
                    }
                }
                else if (existGadget.SpecificationKeys != null && existGadget.SpecificationKeys.Count > 0)
                {
                    var gadgetSpecificationKeys = existGadget.SpecificationKeys;
                    string keys = "";
                    foreach (var key in gadgetSpecificationKeys)
                    {
                        string values = "";
                        foreach (var value in key.SpecificationValues)
                        {
                            if (!value.Value.IsNullOrEmpty())
                            {
                                values += (" " + value.Value.Trim().ToLower());
                            }
                        }
                        if (!key.Name.IsNullOrEmpty())
                        {
                            keys += (" " + key.Name.Trim().ToLower() + values);
                        }
                    }
                    if (!keys.IsNullOrEmpty())
                    {
                        var gadgetVector = await embeddingService.GetEmbedding(existGadget.Name.Trim().ToLower() + " " + keys.Trim());
                        existGadget.Vector = gadgetVector;
                    }
                }
            }
            else
            {
                gadget.BrandId = relatedBrand.Id;
                gadget.CategoryId = relatedCategory.Id;
                gadget.Status = GadgetStatus.Active;
                gadget.CreatedAt = DateTime.UtcNow;
                gadget.UpdatedAt = DateTime.UtcNow;
                gadget.ShopId = shop.Id;

                if (gadget.Specifications != null && gadget.Specifications.Count > 0)
                {
                    string keys = "";
                    foreach (var spe in gadget.Specifications)
                    {
                        var gadgetSpecificationKeys = spe.SpecificationKeys;
                        foreach (var key in gadgetSpecificationKeys)
                        {
                            string values = "";
                            foreach (var value in key.SpecificationValues)
                            {
                                if (!value.Value.IsNullOrEmpty())
                                {
                                    values += (" " + value.Value.Trim().ToLower());
                                }
                            }
                            if (!key.Name.IsNullOrEmpty())
                            {
                                keys += (" " + key.Name.Trim().ToLower() + values); 
                            }
                        }
                    }
                    if (!keys.IsNullOrEmpty())
                    {
                        var gadgetVector = await embeddingService.GetEmbedding(gadget.Name.Trim().ToLower() + " " + keys.Trim());
                        gadget.Vector = gadgetVector;
                    }
                } else if (gadget.SpecificationKeys != null && gadget.SpecificationKeys.Count > 0)
                {
                    var gadgetSpecificationKeys = gadget.SpecificationKeys;
                    string keys = "";
                    foreach (var key in gadgetSpecificationKeys)
                    {
                        string values = "";
                        foreach (var value in key.SpecificationValues)
                        {
                            if (!value.Value.IsNullOrEmpty())
                            {
                                values += (" " + value.Value.Trim().ToLower());
                            }
                        }
                        if (!key.Name.IsNullOrEmpty())
                        {
                            keys += (" " + key.Name.Trim().ToLower() + values);
                        }
                    }
                    if (!keys.IsNullOrEmpty())
                    {
                        var gadgetVector = await embeddingService.GetEmbedding(gadget.Name.Trim().ToLower() + " " + keys.Trim());
                        gadget.Vector = gadgetVector;
                    }
                }

                if (gadget.Vector != null)
                {
                    await context.Gadgets.AddAsync(gadget);
                }
            }

            await context.SaveChangesAsync();
        }
    }

    public string CutUrl(string url)
    {
        var uri = new Uri(url);
        return $"{uri.Scheme}://{uri.Host}";
    }

    public int ConvertPriceToInt(string priceString)
    {
        // Loại bỏ các ký tự không cần thiết như dấu chấm và ký hiệu đồng
        string cleanedString = priceString.Replace(".", "").Replace("₫", "").Trim();

        // Chuyển đổi chuỗi thành số nguyên
        if (int.TryParse(cleanedString, out int price))
        {
            return price;
        }
        else
        {
            return 0;
        }
    }
}
