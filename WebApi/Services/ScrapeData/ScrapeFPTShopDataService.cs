using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PuppeteerSharp;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Background.GadgetScrapeData;
using WebApi.Services.Background.GadgetScrapeData.Models;

namespace WebApi.Services.ScrapeData;

public class ScrapeFPTShopDataService(IOptions<ScrapeDataSettings> scrapeDataSettings, AppDbContext context, GadgetScrapeDataService gadgetScrapeDataService)
{
    private readonly ScrapeDataSettings _scrapeDataSettings = scrapeDataSettings.Value;
    private readonly GadgetScrapeDataService _gadgetScrapeDataService = gadgetScrapeDataService;

    public async Task ScrapeFPTShopGadget()
    {
        var fptShop = await context.Shops
            .FirstOrDefaultAsync(s => s.Name == "FPT Shop");
        if (fptShop == null)
        {
            return;
        }
        List<Category> categories = await context.Categories.ToListAsync();
        foreach (var category in categories)
        {
            var categoriesWithBrands = await context.Categories
                .Where(c => c.Name == category.Name)
                .Include(c => c.CategoryBrands)
                    .ThenInclude(cb => cb.Brand)
                .ToListAsync();
            foreach (var cateWithBrands in categoriesWithBrands)
            {
                switch (cateWithBrands.Name)
                {
                    case "Điện thoại":
                        foreach (var brand in cateWithBrands.Brands)
                        {
                            string phoneUrl = _scrapeDataSettings.FPTShop + "dien-thoai/";
                            switch (brand.Name)
                            {
                                case "Samsung":
                                    phoneUrl += "samsung";
                                    break;
                                case "Apple":
                                    phoneUrl += "apple-iphone";
                                    break;
                                case "Oppo":
                                    phoneUrl += "oppo";
                                    break;
                                case "Xiaomi":
                                    phoneUrl += "xiaomi";
                                    break;
                                case "Vivo":
                                    phoneUrl += "vivo";
                                    break;
                                case "Realme":
                                    phoneUrl += "realme";
                                    break;
                                case "Honor":
                                    phoneUrl += "honor";
                                    break;
                                case "Tecno":
                                    phoneUrl += "tecno";
                                    break;
                                case "Nokia":
                                    phoneUrl += "nokia";
                                    break;
                                case "Masstel":
                                    phoneUrl += "masstel";
                                    break;
                                case "Mobell":
                                    phoneUrl += "mobell";
                                    break;
                                case "Itel":
                                    phoneUrl += "itel";
                                    break;
                                case "Viettel":
                                    phoneUrl += "viettel";
                                    break;
                                case "Benco":
                                    phoneUrl += "benco";
                                    break;
                                case "Inoi":
                                    phoneUrl += "inoi";
                                    break;
                                case "ZTE":
                                    phoneUrl += "zte";
                                    break;
                                default:
                                    continue;
                            }

                            List<Gadget> listGadgets = new List<Gadget>()!;
                            try
                            {
                                listGadgets = await ScrapeGadgetByBrand(phoneUrl);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Điện thoại {brand.Name}: {ex}");
                                continue;
                            }
                            await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context);
                        }
                        break;
                    case "Laptop":
                        foreach (var brand in cateWithBrands.Brands)
                        {
                            string laptopUrl = _scrapeDataSettings.FPTShop + "may-tinh-xach-tay/";
                            switch (brand.Name)
                            {
                                case "Hp":
                                    laptopUrl += "hp";
                                    break;
                                case "Asus":
                                    laptopUrl += "asus";
                                    break;
                                case "Acer":
                                    laptopUrl += "acer";
                                    break;
                                case "Lenovo":
                                    laptopUrl += "lenovo";
                                    break;
                                case "Dell":
                                    laptopUrl += "dell";
                                    break;
                                case "MSI":
                                    laptopUrl += "msi";
                                    break;
                                case "Apple":
                                    laptopUrl += "apple-macbook";
                                    break;
                                case "Gigabyte":
                                    laptopUrl += "gigabyte";
                                    break;
                                case "Huawei":
                                    laptopUrl += "huawei";
                                    break;
                                case "Masstel":
                                    laptopUrl += "masstel";
                                    break;
                                case "Vaio":
                                    laptopUrl += "vaio";
                                    break;
                                default:
                                    continue;
                            }

                            List<Gadget> listGadgets = new List<Gadget>()!;
                            try
                            {
                                listGadgets = await ScrapeGadgetByBrand(laptopUrl);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Laptop {brand.Name}: {ex}");
                                continue;
                            }
                            await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context);
                        }
                        break;
                    case "Thiết bị âm thanh":
                        foreach (var brand in cateWithBrands.Brands)
                        {
                            string soundUrl = _scrapeDataSettings.FPTShop;
                            bool isCaseNormal = true;

                            //Case brand có tai nghe hoặc loa
                            switch (brand.Name)
                            {
                                case "Devia":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=devia";
                                    break;
                                case "Rock":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=rock";
                                    break;
                                case "Sennheiser":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=sennheiser";
                                    break;
                                case "Logitech":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=logitech";
                                    break;
                                case "Pro One":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=pro-one";
                                    break;
                                case "Belkin":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=belkin";
                                    break;
                                case "Rapoo":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=rapoo";
                                    break;
                                case "Soundcore":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=soundcore";
                                    break;
                                case "Zadez":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=zadez";
                                    break;
                                case "Remax":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=remax";
                                    break;
                                case "ProLink":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=prolink";
                                    break;
                                case "Corsair":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=corsair";
                                    break;
                                case "UmeTravel":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=umetravel";
                                    break;
                                case "Defunc":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=defunc";
                                    break;
                                case "HyperX":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=hyperx";
                                    break;
                                case "Oppo":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=oppo";
                                    break;
                                case "KZ":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=kz";
                                    break;
                                case "F.Power":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=f-power";
                                    break;
                                case "Bagitech":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=bagitech";
                                    break;
                                case "Aukey":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=aukey";
                                    break;
                                case "Steelseries":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=steelseries";
                                    break;
                                case "Baseus":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=baseus";
                                    break;
                                case "Soundpeats":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=soundpeats";
                                    break;
                                case "Klatre":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=klatre";
                                    break;
                                case "Apple":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=apple";
                                    break;
                                case "Pioneer":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=pioneer";
                                    break;
                                case "Edifier":
                                    soundUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=edifier";
                                    break;
                                case "Harman Kardon":
                                    soundUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=harman-kardon";
                                    break;
                                case "Microlab":
                                    soundUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=microlab";
                                    break;
                                case "Divoom":
                                    soundUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=divoom";
                                    break;
                                case "Alpha Works":
                                    soundUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=alpha-works";
                                    break;
                                default:
                                    isCaseNormal = false;
                                    break;

                            }
                            if (isCaseNormal)
                            {
                                List<Gadget> listGadgets = new List<Gadget>()!;
                                try
                                {
                                    listGadgets = await ScrapeGadgetByBrand(soundUrl);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Thiết bị âm thanh {brand.Name}: {ex}");
                                    continue;
                                }

                                await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context);
                                continue;
                            }
                            List<string> listSoundUrls = new List<string>()!;
                            string earPhoneUrl = soundUrl;
                            string loudspeakerUrl = soundUrl;

                            //Case brand vừa có tai nghe vừa có loa
                            switch (brand.Name)
                            {
                                case "JBL":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=jbl";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=jbl";
                                    break;
                                case "Havit":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=havit";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=havit";
                                    break;
                                case "Bose":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=bose";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=bose";
                                    break;
                                case "Soundmax":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=soundmax";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=soundmax";
                                    break;
                                case "Pack&Go":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=packgo";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=packgo";
                                    break;
                                case "IValue":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=ivalue";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=ivalue";
                                    break;
                                case "Sony":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=sony";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=sony";
                                    break;
                                case "Marshall":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=marshall";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=marshall";
                                    break;
                                case "Xiaomi":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=xiaomi";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=xiaomi";
                                    break;
                                case "ICore":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=icore";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=icore";
                                    break;
                                case "Samsung":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=samsung";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=samsung";
                                    break;
                                case "Anker":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=anker";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=anker";
                                    break;
                                default:
                                    continue;
                            }

                            listSoundUrls.Add(earPhoneUrl);
                            listSoundUrls.Add(loudspeakerUrl);
                            if (listSoundUrls.Count > 0)
                            {
                                foreach (var urlItem in listSoundUrls)
                                {
                                    List<Gadget> listGadgets = new List<Gadget>()!;
                                    try
                                    {
                                        listGadgets = await ScrapeGadgetByBrand(urlItem);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Thiết bị âm thanh {brand.Name}: {ex}");
                                        continue;
                                    }

                                    await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public async Task<List<Gadget>> ScrapeGadgetByBrand(string url)
    {
        string defaultUrl = _gadgetScrapeDataService.CutUrl(url);
        // Tải xuống Chromium nếu chưa có
        await new BrowserFetcher().DownloadAsync();

        // Khởi tạo trình duyệt
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true, // Chạy ở chế độ headless để không hiển thị giao diện
            Args = ["--no-sandbox"]
        });

        // Mở một tab mới
        var page = await browser.NewPageAsync();

        // Điều hướng đến trang web
        await page.GoToAsync(url);

        bool moreDataToLoad = true;

        //Ấn xem thêm cho đến khi bung hết danh sách gadget
        while (moreDataToLoad)
        {
            try
            {
                var button = await page.WaitForSelectorAsync("button.Button_root__LQsbl.Button_btnSmall__aXxTy.Button_whitePrimary__nkoMI.Button_btnIconRight__4VSUO.border.border-iconDividerOnWhite.px-4.py-2", new WaitForSelectorOptions { Timeout = 2000 });

                if (button != null)
                {
                    Console.WriteLine("Xem thêm gadget");
                    var isButtonVisible = await button.IsVisibleAsync();
                    if (isButtonVisible)
                    {
                        await button.PressAsync("Enter");
                        await Task.Delay(2000);
                    }
                    else
                    {
                        moreDataToLoad = false;
                    }
                }
                else
                {
                    moreDataToLoad = false;
                }
            }
            catch (PuppeteerException ex)
            {
                Console.WriteLine("catch " + ex.Message);
                Console.WriteLine("Hết sản phẩm");
                moreDataToLoad = false;
            }
        }

        //Scrape Price, ThumbnailUrl, NavigationUrl
        string jsonGadgets = await page.EvaluateExpressionAsync<string>(
        @"
            (function () {
                const listGadgetsLi = Array.from(document.querySelectorAll('div.ProductCard_brandCard__VQQT8.ProductCard_cardDefault__km9c5'));

                const newGadgetItemJsonResponses = [];
                for (let i = 0; i < listGadgetsLi.length; i++) {
                    const li = listGadgetsLi[i];
                    const priceElement = li.querySelector('div.ProductCard_cardInfo__r8zG4 p.Price_currentPrice__PBYcv');
                    const GadgetItemJsonResponse = {
                        price: priceElement ? priceElement.textContent : null,
                        thumbnailUrl: null,
                        navigationUrl: null,
                    };

                    const aElement = li.querySelector('a');
                    if (aElement) {
                        GadgetItemJsonResponse.navigationUrl = aElement.getAttribute('href');
                    }

                    const imgElement = li.querySelector('img');
                    if (imgElement) {
                        GadgetItemJsonResponse.thumbnailUrl = imgElement.getAttribute('src');
                    }

                    if (GadgetItemJsonResponse.price || GadgetItemJsonResponse.thumbnailUrl) {
                        newGadgetItemJsonResponses.push(GadgetItemJsonResponse);
                    }
                }
                return JSON.stringify(newGadgetItemJsonResponses);
            })()
        ");
        List<GadgetFPTShopItemJsonResponse> GadgetItemJsonResponses = JsonConvert.DeserializeObject<List<GadgetFPTShopItemJsonResponse>>(jsonGadgets)!;

        var listGadgets = new List<Gadget>();
        for (int i = 0; i < GadgetItemJsonResponses.Count; i++)
        {
            //Điều hướng sang gadget detail
            await page.CloseAsync();
            page = await browser.NewPageAsync();
            await page.GoToAsync(defaultUrl + GadgetItemJsonResponses[i].NavigationUrl);
            await Task.Delay(2000);

            Console.WriteLine("gadget detail url: " + defaultUrl + GadgetItemJsonResponses[i].NavigationUrl);
            Console.WriteLine("tao gadget: " + (i + 1));

            //Kiểm tra xem gadget detail này có nhiều gadget khác không. Ví dụ 128GB, 256GB, 1TB,...
            int numberOfRelatedGadget;
            try
            {
                numberOfRelatedGadget = await page.EvaluateExpressionAsync<int>(
                    @"
                        (function () {
                            const listSpecifications = Array.from(document.querySelectorAll('button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_buttonSelect__7lW_h.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy, button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy'));
                            return listSpecifications.length;
                        })()
                    ");
                //TH không có phần tử con
                if (numberOfRelatedGadget == 0)
                {
                    Gadget gadgetDetail = new Gadget()!;

                    //Get price
                    string jsonGadgetPrice;
                    try
                    {
                        jsonGadgetPrice = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const gadgetPriceElement = document.querySelector('span.text-black-opacity-100.h4-bold');
                                const gadgetPrice = gadgetPriceElement ? gadgetPriceElement.textContent : null;
                                return gadgetPrice;
                            })()
                        ");
                        if (!string.IsNullOrEmpty(jsonGadgetPrice))
                        {
                            gadgetDetail.Price = _gadgetScrapeDataService.ConvertPriceToInt(jsonGadgetPrice);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Get name
                    string jsonGadgetName;
                    try
                    {
                        jsonGadgetName = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const gadgetNameElement = document.querySelector('h1.text-textOnWhitePrimary.b1-semibold');
                                const gadgetName = gadgetNameElement ? gadgetNameElement.textContent : 'N/A';
                                return gadgetName;
                            })()
                        ");
                        if (!string.IsNullOrEmpty(jsonGadgetName))
                        {
                            gadgetDetail.Name = jsonGadgetName;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Get gadget descriptions
                    string jsonGadgetDescriptions;
                    try
                    {
                        jsonGadgetDescriptions = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const thongTinSanPhamDiv = document.getElementById('ThongTinSanPham');

                                if (thongTinSanPhamDiv) {
                                    // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
                                    const elements = thongTinSanPhamDiv.querySelectorAll('h1, h2, h3, h4, p, img');

                                    // Tạo một mảng để lưu nội dung các thẻ
                                    const contentList = [];

                                    // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
                                    for (let element of elements) {
                                        // Nếu thỏa điều kiện để thoát vòng lặp
                                        if (element.tagName.toLowerCase() === 'p' || element.tagName.toLowerCase() === 'h1' || element.tagName.toLowerCase() === 'h2' || element.tagName.toLowerCase() === 'h3' || element.tagName.toLowerCase() === 'h4') {
                                            if (element.innerText && element.innerText == 'Bài viết liên quan') {
                                                break; // Thoát khỏi vòng lặp hoàn toàn
                                            }
                                        }
                                        if (element.tagName.toLowerCase() === 'img') {
                                            contentList.push({ type: 'Image', value: element.src });
                                        } else if (element.tagName.toLowerCase() === 'p') {
                                            if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
                                                contentList.push({ type: 'NormalText', value: element.innerText.replace(/[“”]/g, ""'"") });
                                            }
                                        } else {
                                            if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
                                                contentList.push({ type: 'BoldText', value: element.innerText.replace(/[“”]/g, ""'"") });
                                            }
                                        }
                                    }

                                    return JSON.stringify(contentList); // Trả về mảng chứa nội dung của các thẻ
                                } else {
                                    console.log('Không tìm thấy thẻ div với id ""ThongTinSanPham"".');
                                    return JSON.stringify([]);
                                }
                            })()
                        ");
                        List<GadgetDescription> gadgetDescriptions = JsonConvert.DeserializeObject<List<GadgetDescription>>(jsonGadgetDescriptions)!;
                        if (gadgetDescriptions.Count > 0)
                        {
                            gadgetDetail.GadgetDescriptions = gadgetDescriptions;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Get gadget images
                    string jsonGadgetImages;
                    try
                    {
                        jsonGadgetImages = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
                                const elements = document.querySelectorAll('div.swiper div.swiper-wrapper div.swiper-slide.Slideshow_swiperSlideCustom___6FRx img');
                                if (elements) {

                                    // Tạo một mảng để lưu nội dung các thẻ
                                    const imgList = [];

                                    // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
                                    for (let element of elements) {
                                        const gadgetImage = {
                                            imageUrl: element.getAttribute('src')
                                        }
                                        if (gadgetImage.imageUrl) {
                                            imgList.push(gadgetImage);
                                        }
                                    }

                                    return JSON.stringify(imgList); // Trả về mảng chứa nội dung của các thẻ
                                } else {
                                    console.log('Không tìm thấy thẻ div');
                                    return JSON.stringify([]);
                                }
                            })()
                        ");
                        List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(jsonGadgetImages)!;
                        if (gadgetImages.Count > 0)
                        {
                            gadgetDetail.GadgetImages = gadgetImages;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Click view thông số kỹ thuật detail
                    moreDataToLoad = true;
                    while (moreDataToLoad)
                    {
                        try
                        {
                            var button = await page.WaitForSelectorAsync("button.flex.items-center.text-blue-blue-7.b2-medium", new WaitForSelectorOptions { Timeout = 2000 });

                            if (button != null)
                            {
                                Console.WriteLine("Xem chi tiết thông số kỹ thuật");
                                var isButtonVisible = await button.IsVisibleAsync();
                                if (isButtonVisible)
                                {
                                    await button.PressAsync("Enter");
                                    await Task.Delay(2000);
                                    try
                                    {
                                        await page.WaitForSelectorAsync("div.Sheet_sheet__59S7D", new WaitForSelectorOptions { Timeout = 2000 });
                                        moreDataToLoad = false;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("popup catch " + ex.Message);
                                        moreDataToLoad = true;
                                    }
                                }
                                else
                                {
                                    moreDataToLoad = false;
                                }
                            }
                            else
                            {
                                moreDataToLoad = false;
                            }
                        }
                        catch (PuppeteerException ex)
                        {
                            Console.WriteLine("catch " + ex.Message);
                            moreDataToLoad = false;
                        }
                    }

                    //Get gadget specifications
                    string jsonGadgetSpecifications;
                    try
                    {
                        jsonGadgetSpecifications = await page.EvaluateExpressionAsync<string>(
                        @"
                        (function () {
                            const specifications = [];

                            const specificationElements = document.querySelectorAll('div.Sheet_sheet__59S7D div.Sheet_body__VKc95.active div.tab-content.flex.flex-col.pt-5');

                            for (let i = 0; i < specificationElements.length; i++) {
                                const specificationElement = specificationElements[i].querySelector('div.mb-1.flex.items-center.gap-2.text-textOnWhitePrimary.b2-semibold span');
                                const specificationName = specificationElement?.textContent?.trim();
                                if (specificationName == null || specificationName === '') {
                                    continue;
                                }
                                const specification = {
                                    name: specificationName,
                                    specificationKeys: []
                                };
                                const rowsElement = specificationElements[i].querySelectorAll('div.flex.gap-2.border-b');
                                for (let j = 0; j < rowsElement.length; j++) {
                                    const specificationKey = {
                                        name: null,
                                        specificationValues: []
                                    }
                                    const specificationKeyName = rowsElement[j].querySelector('div.text-textOnWhiteSecondary.b2-regular span').textContent.trim();
                                    if (specificationKeyName) {
                                        specificationKey.name = specificationKeyName;
                                    } else {
                                        continue;
                                    }

                                    // Kiểm tra xem value là span hay p
                                    const spanValue = rowsElement[j].querySelector('span.flex-1.text-textOnWhitePrimary.b2-regular');
                                    const pValues = rowsElement[j].querySelectorAll('div.flex.flex-1.flex-col p');

                                    const specificationValue = {
                                        value: null,
                                    }

                                    if (spanValue) {
                                        // Nếu chỉ có 1 value (thẻ span)
                                        specificationValue.value = spanValue.textContent.trim();
                                        specificationKey.specificationValues.push(specificationValue);
                                    } else if (pValues.length > 0) {
                                        // Nếu có nhiều value (các thẻ p)
                                        for (let k = 0; k < pValues.length; k++) {
                                            const specificationValue = {
                                                value: null,
                                            }
                                            specificationValue.value = pValues[k].textContent.trim();
                                            specificationKey.specificationValues.push(specificationValue);
                                        }
                                    } else {
                                        continue;
                                    }

                                    specification.specificationKeys.push(specificationKey);
                                }

                                specifications.push(specification);
                            }

                            return JSON.stringify(specifications);
                        })()
                    ");
                        List<Specification> gadgetSpecifications = JsonConvert.DeserializeObject<List<Specification>>(jsonGadgetSpecifications)!;
                        if (gadgetSpecifications.Count > 0)
                        {
                            gadgetDetail.Specifications = gadgetSpecifications;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    listGadgets.Add(gadgetDetail);

                    Console.WriteLine(JsonConvert.SerializeObject(gadgetDetail));
                    Console.WriteLine("add success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Số phần tử con không hợp lệ: {ex.Message}");
                continue;
            }

            //TH có nhiều phần tử con
            for (int j = 0; j < numberOfRelatedGadget; j++)
            {
                Console.WriteLine("tao gadget lien quan: " + (j + 1));
                Gadget gadgetDetail = new Gadget()!;

                //Get price
                string jsonGadgetPrice;
                try
                {
                    jsonGadgetPrice = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const gadgetPriceElement = document.querySelector('span.text-black-opacity-100.h4-bold');
                            const gadgetPrice = gadgetPriceElement ? gadgetPriceElement.textContent : null;
                            return gadgetPrice;
                        })()
                    ");
                    if (!string.IsNullOrEmpty(jsonGadgetPrice))
                    {
                        gadgetDetail.Price = _gadgetScrapeDataService.ConvertPriceToInt(jsonGadgetPrice);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Get name
                string jsonGadgetName;
                try
                {
                    jsonGadgetName = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const gadgetNameElement = document.querySelector('h1.text-textOnWhitePrimary.b1-semibold');
                            const gadgetName = gadgetNameElement ? gadgetNameElement.textContent : 'N/A';
                            return gadgetName;
                        })()
                    ");
                    if (!string.IsNullOrEmpty(jsonGadgetName))
                    {
                        gadgetDetail.Name = jsonGadgetName;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Get gadget descriptions
                string jsonGadgetDescriptions;
                try
                {
                    jsonGadgetDescriptions = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const thongTinSanPhamDiv = document.getElementById('ThongTinSanPham');

                            if (thongTinSanPhamDiv) {
                                // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
                                const elements = thongTinSanPhamDiv.querySelectorAll('h1, h2, h3, h4, p, img');

                                // Tạo một mảng để lưu nội dung các thẻ
                                const contentList = [];

                                // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
                                for (let element of elements) {
                                    // Nếu thỏa điều kiện để thoát vòng lặp
                                    if (element.tagName.toLowerCase() === 'p' || element.tagName.toLowerCase() === 'h1' || element.tagName.toLowerCase() === 'h2' || element.tagName.toLowerCase() === 'h3' || element.tagName.toLowerCase() === 'h4') {
                                        if (element.innerText && element.innerText == 'Bài viết liên quan') {
                                            break; // Thoát khỏi vòng lặp hoàn toàn
                                        }
                                    }
                                    if (element.tagName.toLowerCase() === 'img') {
                                        contentList.push({ type: 'Image', value: element.src });
                                    } else if (element.tagName.toLowerCase() === 'p') {
                                        if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
                                            contentList.push({ type: 'NormalText', value: element.innerText.replace(/[“”]/g, ""'"") });
                                        }
                                    } else {
                                        if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
                                            contentList.push({ type: 'BoldText', value: element.innerText.replace(/[“”]/g, ""'"") });
                                        }
                                    }
                                }

                                return JSON.stringify(contentList); // Trả về mảng chứa nội dung của các thẻ
                            } else {
                                console.log('Không tìm thấy thẻ div với id ""ThongTinSanPham"".');
                                return JSON.stringify([]);
                            }
                        })()
                    ");
                    List<GadgetDescription> gadgetDescriptions = JsonConvert.DeserializeObject<List<GadgetDescription>>(jsonGadgetDescriptions)!;
                    if (gadgetDescriptions.Count > 0)
                    {
                        gadgetDetail.GadgetDescriptions = gadgetDescriptions;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Get gadget images
                string jsonGadgetImages;
                try
                {
                    jsonGadgetImages = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
                            const elements = document.querySelectorAll('div.swiper div.swiper-wrapper div.swiper-slide.Slideshow_swiperSlideCustom___6FRx img');
                            if (elements) {

                                // Tạo một mảng để lưu nội dung các thẻ
                                const imgList = [];

                                // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
                                for (let element of elements) {
                                    const gadgetImage = {
                                        imageUrl: element.getAttribute('src')
                                    }
                                    if (gadgetImage.imageUrl) {
                                        imgList.push(gadgetImage);
                                    }
                                }

                                return JSON.stringify(imgList); // Trả về mảng chứa nội dung của các thẻ
                            } else {
                                console.log('Không tìm thấy thẻ div');
                                return JSON.stringify([]);
                            }
                        })()
                    ");
                    List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(jsonGadgetImages)!;
                    if (gadgetImages.Count > 0)
                    {
                        gadgetDetail.GadgetImages = gadgetImages;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Click view thông số kỹ thuật detail
                moreDataToLoad = true;
                while (moreDataToLoad)
                {
                    try
                    {
                        var button = await page.WaitForSelectorAsync("button.flex.items-center.text-blue-blue-7.b2-medium", new WaitForSelectorOptions { Timeout = 2000 });

                        if (button != null)
                        {
                            Console.WriteLine("Xem chi tiết thông số kỹ thuật");
                            var isButtonVisible = await button.IsVisibleAsync();
                            if (isButtonVisible)
                            {
                                await button.PressAsync("Enter");
                                await Task.Delay(2000);
                                try
                                {
                                    await page.WaitForSelectorAsync("div.Sheet_sheet__59S7D", new WaitForSelectorOptions { Timeout = 2000 });
                                    moreDataToLoad = false;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("popup catch " + ex.Message);
                                    moreDataToLoad = true;
                                }
                            }
                            else
                            {
                                moreDataToLoad = false;
                            }
                        }
                        else
                        {
                            moreDataToLoad = false;
                        }
                    }
                    catch (PuppeteerException ex)
                    {
                        Console.WriteLine("catch " + ex.Message);
                        moreDataToLoad = false;
                    }
                }

                //Get gadget specifications
                string jsonGadgetSpecifications;
                try
                {
                    jsonGadgetSpecifications = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const specifications = [];

                            const specificationElements = document.querySelectorAll('div.Sheet_sheet__59S7D div.Sheet_body__VKc95.active div.tab-content.flex.flex-col.pt-5');

                            for (let i = 0; i < specificationElements.length; i++) {
                                const specificationElement = specificationElements[i].querySelector('div.mb-1.flex.items-center.gap-2.text-textOnWhitePrimary.b2-semibold span');
                                const specificationName = specificationElement?.textContent?.trim();
                                if (specificationName == null || specificationName === '') {
                                    continue;
                                }
                                const specification = {
                                    name: specificationName,
                                    specificationKeys: []
                                };
                                const rowsElement = specificationElements[i].querySelectorAll('div.flex.gap-2.border-b');
                                for (let j = 0; j < rowsElement.length; j++) {
                                    const specificationKey = {
                                        name: null,
                                        specificationValues: []
                                    }
                                    const specificationKeyName = rowsElement[j].querySelector('div.text-textOnWhiteSecondary.b2-regular span').textContent.trim();
                                    if (specificationKeyName) {
                                        specificationKey.name = specificationKeyName;
                                    } else {
                                        continue;
                                    }

                                    // Kiểm tra xem value là span hay p
                                    const spanValue = rowsElement[j].querySelector('span.flex-1.text-textOnWhitePrimary.b2-regular');
                                    const pValues = rowsElement[j].querySelectorAll('div.flex.flex-1.flex-col p');

                                    const specificationValue = {
                                        value: null,
                                    }

                                    if (spanValue) {
                                        // Nếu chỉ có 1 value (thẻ span)
                                        specificationValue.value = spanValue.textContent.trim();
                                        specificationKey.specificationValues.push(specificationValue);
                                    } else if (pValues.length > 0) {
                                        // Nếu có nhiều value (các thẻ p)
                                        for (let k = 0; k < pValues.length; k++) {
                                            const specificationValue = {
                                                value: null,
                                            }
                                            specificationValue.value = pValues[k].textContent.trim();
                                            specificationKey.specificationValues.push(specificationValue);
                                        }
                                    } else {
                                        continue;
                                    }

                                    specification.specificationKeys.push(specificationKey);
                                }

                                specifications.push(specification);
                            }

                            return JSON.stringify(specifications);
                        })()
                    ");
                    List<Specification> gadgetSpecifications = JsonConvert.DeserializeObject<List<Specification>>(jsonGadgetSpecifications)!;
                    if (gadgetSpecifications.Count > 0)
                    {
                        gadgetDetail.Specifications = gadgetSpecifications;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                listGadgets.Add(gadgetDetail);

                Console.WriteLine(JsonConvert.SerializeObject(gadgetDetail));
                Console.WriteLine("add success");

                //Click nút close popup
                moreDataToLoad = true;
                while (moreDataToLoad && j < numberOfRelatedGadget - 1)
                {
                    try
                    {
                        var clsButton = await page.WaitForSelectorAsync("svg.Sheet_icon__RnybF", new WaitForSelectorOptions { Timeout = 2000 });

                        if (clsButton != null)
                        {
                            var isButtonVisible = await clsButton.IsVisibleAsync();
                            if (isButtonVisible)
                            {
                                Console.WriteLine("clsButton visible");
                                await clsButton.ClickAsync();
                                await Task.Delay(2000);
                                try
                                {
                                    await page.WaitForSelectorAsync("div.Sheet_sheet__59S7D", new WaitForSelectorOptions { Timeout = 2000 });
                                    moreDataToLoad = true;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("popup catch " + ex.Message);
                                    Console.WriteLine("Đã đón popup");
                                    moreDataToLoad = false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("clsButton invisible");
                                moreDataToLoad = false;
                            }
                        }
                        else
                        {
                            moreDataToLoad = false;
                        }
                    }
                    catch (PuppeteerException ex)
                    {
                        Console.WriteLine("clsButton catch " + ex.Message);
                        moreDataToLoad = false;
                    }
                }

                //Click next gadget detail
                if (j < numberOfRelatedGadget - 1)
                {
                    bool isNotClicked = true;
                    while (isNotClicked)
                    {
                        try
                        {
                            // Lấy tất cả các nút có class 'Selection_button__vX7ZX'
                            var buttons = await page.QuerySelectorAllAsync("button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_buttonSelect__7lW_h.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy, button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy");

                            await buttons[j + 1].PressAsync("Enter");
                            await Task.Delay(2000);
                            try
                            {
                                isNotClicked = !await page.EvaluateExpressionAsync<bool>(
                                $@"
                                    (function () {{
                                        const listBtn = document.querySelectorAll('button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_buttonSelect__7lW_h.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy, button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy');
                                        const isClicked = listBtn[{j + 1}].classList.contains('Selection_buttonSelect__7lW_h');
                                        return isClicked;
                                    }})()
                                ");
                                if (isNotClicked)
                                {
                                    Console.WriteLine($"Nút {j + 1} chưa được nhấn");
                                }
                                else
                                {
                                    Console.WriteLine($"Đã nhấn vào nút {j + 1}");
                                    await Task.Delay(2000);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                isNotClicked = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                            isNotClicked = true;
                        }
                    }
                }
            }
        }
        await browser.CloseAsync();

        return listGadgets;
    }
}