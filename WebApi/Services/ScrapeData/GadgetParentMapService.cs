using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services.ScrapeData;

public class GadgetParentMapService(AppDbContext context)
{
    public async Task MapGadget()
    {
        var shopList = await context.Shops.ToListAsync();
        var cateList = await context.Categories.ToListAsync();

        foreach (var shop in shopList)
        {
            foreach (var cate in cateList)
            {
                var gadgetsInShop = await context.Gadgets
                    .Where(g => (g.ShopId == shop.Id && g.CategoryId == cate.Id))
                    .Include(g => g.Brand)
                    .ToListAsync();
                foreach (var gadget in gadgetsInShop)
                {
                    //Lấy gadget không phải shop hiện tại, chung category, chung brand và chưa có cha
                    var gadgetsInOtherShops = await context.Gadgets
                        .Where(g => g.ShopId != shop.Id && g.CategoryId == cate.Id && g.BrandId == gadget.BrandId)
                        .Include(g => g.Shop)
                        .Include(g => g.Brand)
                        .ToListAsync();
                    foreach (var gadgetOther in gadgetsInOtherShops)
                    {
                        var score = Fuzz.Ratio(gadget.Name, gadgetOther.Name);
                        Console.WriteLine($"Gad 1: {gadget.Name}, Gad 2: {gadgetOther.Name}. Score: {score}");
                        bool isWithinRange = IsPriceWithinPercentageDifference(gadget.Price, gadgetOther.Price, 0.15);
                        int targetScore = 0;
                        switch (cate.Name)
                        {
                            case "Điện thoại":
                                if (shop.Name == "Thế Giới Di Động" && gadgetOther.Shop!.Name == "FPT Shop")
                                {
                                    targetScore = 79;
                                }
                                else if (shop.Name == "Thế Giới Di Động" && gadgetOther.Shop!.Name == "Phong Vũ")
                                {

                                }
                                else if (shop.Name == "FPT Shop" && gadgetOther.Shop!.Name == "Phong Vũ")
                                {

                                }
                                break;
                            case "Laptop":
                                if (shop.Name == "Thế Giới Di Động" && gadgetOther.Shop!.Name == "FPT Shop")
                                {
                                    targetScore = 79;
                                }
                                else if (shop.Name == "Thế Giới Di Động" && gadgetOther.Shop!.Name == "Phong Vũ")
                                {

                                }
                                else if (shop.Name == "FPT Shop" && gadgetOther.Shop!.Name == "Phong Vũ")
                                {

                                }
                                break;
                            case "Thiết bị âm thanh":
                                if (shop.Name == "Thế Giới Di Động" && gadgetOther.Shop!.Name == "FPT Shop")
                                {
                                    score = Fuzz.Ratio(RemoveCommonWords(gadget.Name), RemoveCommonWords(gadgetOther.Name));
                                    Console.WriteLine($"Gad 1: {RemoveCommonWords(gadget.Name)}, Gad 2: {RemoveCommonWords(gadgetOther.Name)}. Score: {score}");

                                    if (gadget.Brand!.Name == "Apple" && gadgetOther.Brand!.Name == "Apple")
                                    {
                                        targetScore = 55;
                                    }
                                    else
                                    {
                                        targetScore = 95;
                                    }
                                }
                                else if (shop.Name == "Thế Giới Di Động" && gadgetOther.Shop!.Name == "Phong Vũ")
                                {

                                }
                                else if (shop.Name == "FPT Shop" && gadgetOther.Shop!.Name == "Phong Vũ")
                                {

                                }
                                break;
                            default:
                                targetScore = 100;
                                break;
                        }
                        if (score >= targetScore && isWithinRange)
                        {
                            //gadgetOther.ParentId = gadget.Id;
                            await context.SaveChangesAsync();
                            Console.WriteLine($"Match!");
                        }
                    }
                }
            }
        }
    }
    private static string RemoveCommonWords(string input)
    {
        string[] commonWords = { "Bluetooth", "cao cấp", "không dây", "chính hãng", "Chụp tai", "Nhét tai", "Choàng đầu", "True Wireless", ".", "Có Dây", "Phụ kiện nhập khẩu", "Chủ Động Khử Tiếng Ồn", "Hộp sạc dây", "cổng USB C", "USB-C", "(chống ồn)" };
        foreach (var word in commonWords)
        {
            input = input.Replace(word, "", StringComparison.OrdinalIgnoreCase).Trim();
        }
        return input;
    }

    public bool IsPriceWithinPercentageDifference(decimal? price1, decimal? price2, double percentage)
    {
        // Check if either of the prices is null
        if (!price1.HasValue || !price2.HasValue)
            return false;

        // Calculate the absolute difference between the two prices
        var priceDifference = Math.Abs(price1.Value - price2.Value);

        // Calculate the maximum allowable difference based on the percentage (in your case 0.1%)
        var maxAllowedDifference = Math.Max(price1.Value, price2.Value) * (decimal)(percentage / 100);

        // Check if the actual difference is within the allowable range
        return priceDifference <= maxAllowedDifference;
    }
}
