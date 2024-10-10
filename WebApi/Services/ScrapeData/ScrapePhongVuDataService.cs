using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PuppeteerSharp;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Background.GadgetScrapeData;
using WebApi.Services.Background.GadgetScrapeData.Models;
using WebApi.Services.Embedding;

namespace WebApi.Services.ScrapeData;

public class ScrapePhongVuDataService(IOptions<ScrapeDataSettings> scrapeDataSettings, AppDbContext context, GadgetScrapeDataService gadgetScrapeDataService, EmbeddingService embeddingService)
{
    private readonly ScrapeDataSettings _scrapeDataSettings = scrapeDataSettings.Value;
    private readonly GadgetScrapeDataService _gadgetScrapeDataService = gadgetScrapeDataService;
    private readonly EmbeddingService _embeddingService = embeddingService;

    public async Task ScrapePhongVuGadget()
    {
        var fptShop = await context.Shops
            .FirstOrDefaultAsync(s => s.Name == "Phong Vũ");
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
                            //await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context, _embeddingService);
                        }
                        break;
                    case "Laptop":
                        foreach (var brand in cateWithBrands.Brands)
                        {
                            string laptopUrl = _scrapeDataSettings.FPTShop + "may-tinh-xach-tay/";
                            switch (brand.Name)
                            {
                                case "Dell":
                                    laptopUrl += "dell";
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
                            //await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context, _embeddingService);
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
                                case "Oppo":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=oppo";
                                    break;
                                case "Apple":
                                    soundUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=apple";
                                    break;
                                case "Edifier":
                                    soundUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=edifier";
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

                                //await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context, _embeddingService);
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
                                case "Marshall":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=marshall";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=marshall";
                                    break;
                                case "Samsung":
                                    earPhoneUrl += "am-thanh-hinh-anh/tai-nghe?hang-san-xuat=samsung";
                                    loudspeakerUrl += "am-thanh-hinh-anh/loa?hang-san-xuat=samsung";
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

                                    //await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, fptShop, context, _embeddingService);
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
                var button = await page.WaitForSelectorAsync("div.css-1wwh4ob a.css-b0m1yo", new WaitForSelectorOptions { Timeout = 2000 });

                if (button != null)
                {
                    Console.WriteLine("Xem thêm gadget");
                    var isButtonVisible = await button.IsVisibleAsync();
                    if (isButtonVisible)
                    {
                        await button.ClickAsync();
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
                const listGadgetsLi = Array.from(document.querySelectorAll('div.css-1y2krk0 div.css-13w7uog'));

                const newGadgetItemJsonResponses = [];
                for (let i = 0; i < listGadgetsLi.length; i++) {
                    const li = listGadgetsLi[i];
                    const priceDefaultElement = li.querySelector('div.css-kgkvir div.css-1co26wt div.att-product-detail-retail-price.css-18z00w6');
                    const priceElement = li.querySelector('div.css-1co26wt div.att-product-detail-latest-price.css-do31rh');
                    const GadgetItemJsonResponse = {
                        price: priceDefaultElement ? priceDefaultElement.textContent : priceElement ? priceElement.textContent : null,
                        thumbnailUrl: null,
                        navigationUrl: null,
                    };

                    const aElement = li.querySelector('a.css-pxdb0j');
                    if (aElement) {
                        GadgetItemJsonResponse.navigationUrl = aElement.getAttribute('href');
                    }

                    const imgElement = li.querySelector('div.css-4rhdrh div.css-1v97aik div.css-798fc img');
                    if (imgElement) {
                        GadgetItemJsonResponse.thumbnailUrl = imgElement.getAttribute('src');
                    }

                    if (GadgetItemJsonResponse.price || GadgetItemJsonResponse.thumbnailUrl) {
                        if (GadgetItemJsonResponse.price == '') {
                            GadgetItemJsonResponse.price = null;
                        }
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
                            const listSpecifications = Array.from(document.querySelectorAll('div.css-vxzt17 a'));
                            return listSpecifications.length;
                        })()
                    ");
                Console.WriteLine(numberOfRelatedGadget);
                //TH không có phần tử con
                //if (numberOfRelatedGadget == 0)
                //{
                //    Gadget gadgetDetail = new Gadget()!;

                //    //Get price
                //    string jsonGadgetPrice;
                //    try
                //    {
                //        jsonGadgetPrice = await page.EvaluateExpressionAsync<string>(
                //        @"
                //            (function () {
                //                const gadgetPriceElement = document.querySelector('span.text-black-opacity-100.h4-bold');
                //                const gadgetPrice = gadgetPriceElement ? gadgetPriceElement.textContent : null;
                //                return gadgetPrice;
                //            })()
                //        ");
                //        if (!string.IsNullOrEmpty(jsonGadgetPrice))
                //        {
                //            gadgetDetail.Price = _gadgetScrapeDataService.ConvertPriceToInt(jsonGadgetPrice);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }

                //    //Get name
                //    string jsonGadgetName;
                //    try
                //    {
                //        jsonGadgetName = await page.EvaluateExpressionAsync<string>(
                //        @"
                //            (function () {
                //                const gadgetNameElement = document.querySelector('h1.text-textOnWhitePrimary.b1-semibold');
                //                const gadgetName = gadgetNameElement ? gadgetNameElement.textContent : 'N/A';
                //                return gadgetName;
                //            })()
                //        ");
                //        if (!string.IsNullOrEmpty(jsonGadgetName))
                //        {
                //            gadgetDetail.Name = jsonGadgetName;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }

                //    //Get gadget descriptions
                //    string jsonGadgetDescriptions;
                //    try
                //    {
                //        jsonGadgetDescriptions = await page.EvaluateExpressionAsync<string>(
                //        @"
                //            (function () {
                //                const thongTinSanPhamDiv = document.getElementById('ThongTinSanPham');

                //                if (thongTinSanPhamDiv) {
                //                    // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
                //                    const elements = thongTinSanPhamDiv.querySelectorAll('h1, h2, h3, h4, p, img');

                //                    // Tạo một mảng để lưu nội dung các thẻ
                //                    const contentList = [];

                //                    // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
                //                    for (let element of elements) {
                //                        // Nếu thỏa điều kiện để thoát vòng lặp
                //                        if (element.tagName.toLowerCase() === 'p' || element.tagName.toLowerCase() === 'h1' || element.tagName.toLowerCase() === 'h2' || element.tagName.toLowerCase() === 'h3' || element.tagName.toLowerCase() === 'h4') {
                //                            if (element.innerText && element.innerText == 'Bài viết liên quan') {
                //                                break; // Thoát khỏi vòng lặp hoàn toàn
                //                            }
                //                        }
                //                        if (element.tagName.toLowerCase() === 'img') {
                //                            contentList.push({ type: 'Image', value: element.src });
                //                        } else if (element.tagName.toLowerCase() === 'p') {
                //                            if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
                //                                contentList.push({ type: 'NormalText', value: element.innerText.replace(/[“”]/g, ""'"") });
                //                            }
                //                        } else {
                //                            if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
                //                                contentList.push({ type: 'BoldText', value: element.innerText.replace(/[“”]/g, ""'"") });
                //                            }
                //                        }
                //                    }

                //                    return JSON.stringify(contentList); // Trả về mảng chứa nội dung của các thẻ
                //                } else {
                //                    console.log('Không tìm thấy thẻ div với id ""ThongTinSanPham"".');
                //                    return JSON.stringify([]);
                //                }
                //            })()
                //        ");
                //        List<GadgetDescription> gadgetDescriptions = JsonConvert.DeserializeObject<List<GadgetDescription>>(jsonGadgetDescriptions)!;
                //        if (gadgetDescriptions.Count > 0)
                //        {
                //            gadgetDetail.GadgetDescriptions = gadgetDescriptions;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }

                //    //Get gadget images
                //    string jsonGadgetImages;
                //    try
                //    {
                //        jsonGadgetImages = await page.EvaluateExpressionAsync<string>(
                //        @"
                //            (function () {
                //                // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
                //                const elements = document.querySelectorAll('div.swiper div.swiper-wrapper div.swiper-slide.Slideshow_swiperSlideCustom___6FRx img');
                //                if (elements) {

                //                    // Tạo một mảng để lưu nội dung các thẻ
                //                    const imgList = [];

                //                    // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
                //                    for (let element of elements) {
                //                        const gadgetImage = {
                //                            imageUrl: element.getAttribute('src')
                //                        }
                //                        if (gadgetImage.imageUrl) {
                //                            imgList.push(gadgetImage);
                //                        }
                //                    }

                //                    return JSON.stringify(imgList); // Trả về mảng chứa nội dung của các thẻ
                //                } else {
                //                    console.log('Không tìm thấy thẻ div');
                //                    return JSON.stringify([]);
                //                }
                //            })()
                //        ");
                //        List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(jsonGadgetImages)!;
                //        if (gadgetImages.Count > 0)
                //        {
                //            gadgetDetail.GadgetImages = gadgetImages;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }

                //    //Click view thông số kỹ thuật detail
                //    moreDataToLoad = true;
                //    while (moreDataToLoad)
                //    {
                //        try
                //        {
                //            var button = await page.WaitForSelectorAsync("button.flex.items-center.text-blue-blue-7.b2-medium", new WaitForSelectorOptions { Timeout = 2000 });

                //            if (button != null)
                //            {
                //                Console.WriteLine("Xem chi tiết thông số kỹ thuật");
                //                var isButtonVisible = await button.IsVisibleAsync();
                //                if (isButtonVisible)
                //                {
                //                    await button.PressAsync("Enter");
                //                    await Task.Delay(2000);
                //                    try
                //                    {
                //                        await page.WaitForSelectorAsync("div.Sheet_sheet__59S7D", new WaitForSelectorOptions { Timeout = 2000 });
                //                        moreDataToLoad = false;
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Console.WriteLine("popup catch " + ex.Message);
                //                        moreDataToLoad = true;
                //                    }
                //                }
                //                else
                //                {
                //                    moreDataToLoad = false;
                //                }
                //            }
                //            else
                //            {
                //                moreDataToLoad = false;
                //            }
                //        }
                //        catch (PuppeteerException ex)
                //        {
                //            Console.WriteLine("catch " + ex.Message);
                //            moreDataToLoad = false;
                //        }
                //    }

                //    //Get gadget specifications
                //    string jsonGadgetSpecifications;
                //    try
                //    {
                //        jsonGadgetSpecifications = await page.EvaluateExpressionAsync<string>(
                //        @"
                //        (function () {
                //            const specifications = [];

                //            const specificationElements = document.querySelectorAll('div.Sheet_sheet__59S7D div.Sheet_body__VKc95.active div.tab-content.flex.flex-col.pt-5');

                //            for (let i = 0; i < specificationElements.length; i++) {
                //                const specificationElement = specificationElements[i].querySelector('div.mb-1.flex.items-center.gap-2.text-textOnWhitePrimary.b2-semibold span');
                //                const specificationName = specificationElement?.textContent?.trim();
                //                if (specificationName == null || specificationName === '') {
                //                    continue;
                //                }
                //                const specification = {
                //                    name: specificationName,
                //                    specificationKeys: []
                //                };
                //                const rowsElement = specificationElements[i].querySelectorAll('div.flex.gap-2.border-b');
                //                for (let j = 0; j < rowsElement.length; j++) {
                //                    const specificationKey = {
                //                        name: null,
                //                        specificationValues: []
                //                    }
                //                    const specificationKeyName = rowsElement[j].querySelector('div.text-textOnWhiteSecondary.b2-regular span').textContent.trim();
                //                    if (specificationKeyName) {
                //                        specificationKey.name = specificationKeyName;
                //                    } else {
                //                        continue;
                //                    }

                //                    // Kiểm tra xem value là span hay p
                //                    const spanValue = rowsElement[j].querySelector('span.flex-1.text-textOnWhitePrimary.b2-regular');
                //                    const pValues = rowsElement[j].querySelectorAll('div.flex.flex-1.flex-col p');

                //                    const specificationValue = {
                //                        value: null,
                //                    }

                //                    if (spanValue) {
                //                        // Nếu chỉ có 1 value (thẻ span)
                //                        specificationValue.value = spanValue.textContent.trim();
                //                        specificationKey.specificationValues.push(specificationValue);
                //                    } else if (pValues.length > 0) {
                //                        // Nếu có nhiều value (các thẻ p)
                //                        for (let k = 0; k < pValues.length; k++) {
                //                            const specificationValue = {
                //                                value: null,
                //                            }
                //                            specificationValue.value = pValues[k].textContent.trim();
                //                            specificationKey.specificationValues.push(specificationValue);
                //                        }
                //                    } else {
                //                        continue;
                //                    }

                //                    specification.specificationKeys.push(specificationKey);
                //                }

                //                specifications.push(specification);
                //            }

                //            return JSON.stringify(specifications);
                //        })()
                //    ");
                //        List<Specification> gadgetSpecifications = JsonConvert.DeserializeObject<List<Specification>>(jsonGadgetSpecifications)!;
                //        if (gadgetSpecifications.Count > 0)
                //        {
                //            gadgetDetail.Specifications = gadgetSpecifications;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }

                //    listGadgets.Add(gadgetDetail);

                //    Console.WriteLine(JsonConvert.SerializeObject(gadgetDetail));
                //    Console.WriteLine("add success");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Số phần tử con không hợp lệ: {ex.Message}");
                continue;
            }

            //TH có nhiều phần tử con
            //for (int j = 0; j < numberOfRelatedGadget; j++)
            //{
            //    Console.WriteLine("tao gadget lien quan: " + (j + 1));
            //    Gadget gadgetDetail = new Gadget()!;

            //    //Get price
            //    string jsonGadgetPrice;
            //    try
            //    {
            //        jsonGadgetPrice = await page.EvaluateExpressionAsync<string>(
            //        @"
            //            (function () {
            //                const gadgetPriceElement = document.querySelector('span.text-black-opacity-100.h4-bold');
            //                const gadgetPrice = gadgetPriceElement ? gadgetPriceElement.textContent : null;
            //                return gadgetPrice;
            //            })()
            //        ");
            //        if (!string.IsNullOrEmpty(jsonGadgetPrice))
            //        {
            //            gadgetDetail.Price = _gadgetScrapeDataService.ConvertPriceToInt(jsonGadgetPrice);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //    //Get name
            //    string jsonGadgetName;
            //    try
            //    {
            //        jsonGadgetName = await page.EvaluateExpressionAsync<string>(
            //        @"
            //            (function () {
            //                const gadgetNameElement = document.querySelector('h1.text-textOnWhitePrimary.b1-semibold');
            //                const gadgetName = gadgetNameElement ? gadgetNameElement.textContent : 'N/A';
            //                return gadgetName;
            //            })()
            //        ");
            //        if (!string.IsNullOrEmpty(jsonGadgetName))
            //        {
            //            gadgetDetail.Name = jsonGadgetName;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //    //Get gadget descriptions
            //    string jsonGadgetDescriptions;
            //    try
            //    {
            //        jsonGadgetDescriptions = await page.EvaluateExpressionAsync<string>(
            //        @"
            //            (function () {
            //                const thongTinSanPhamDiv = document.getElementById('ThongTinSanPham');

            //                if (thongTinSanPhamDiv) {
            //                    // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
            //                    const elements = thongTinSanPhamDiv.querySelectorAll('h1, h2, h3, h4, p, img');

            //                    // Tạo một mảng để lưu nội dung các thẻ
            //                    const contentList = [];

            //                    // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
            //                    for (let element of elements) {
            //                        // Nếu thỏa điều kiện để thoát vòng lặp
            //                        if (element.tagName.toLowerCase() === 'p' || element.tagName.toLowerCase() === 'h1' || element.tagName.toLowerCase() === 'h2' || element.tagName.toLowerCase() === 'h3' || element.tagName.toLowerCase() === 'h4') {
            //                            if (element.innerText && element.innerText == 'Bài viết liên quan') {
            //                                break; // Thoát khỏi vòng lặp hoàn toàn
            //                            }
            //                        }
            //                        if (element.tagName.toLowerCase() === 'img') {
            //                            contentList.push({ type: 'Image', value: element.src });
            //                        } else if (element.tagName.toLowerCase() === 'p') {
            //                            if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
            //                                contentList.push({ type: 'NormalText', value: element.innerText.replace(/[“”]/g, ""'"") });
            //                            }
            //                        } else {
            //                            if (element.innerText && element.innerText !== '' && element.innerText !== 'Thông tin sản phẩm') {
            //                                contentList.push({ type: 'BoldText', value: element.innerText.replace(/[“”]/g, ""'"") });
            //                            }
            //                        }
            //                    }

            //                    return JSON.stringify(contentList); // Trả về mảng chứa nội dung của các thẻ
            //                } else {
            //                    console.log('Không tìm thấy thẻ div với id ""ThongTinSanPham"".');
            //                    return JSON.stringify([]);
            //                }
            //            })()
            //        ");
            //        List<GadgetDescription> gadgetDescriptions = JsonConvert.DeserializeObject<List<GadgetDescription>>(jsonGadgetDescriptions)!;
            //        if (gadgetDescriptions.Count > 0)
            //        {
            //            gadgetDetail.GadgetDescriptions = gadgetDescriptions;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //    //Get gadget images
            //    string jsonGadgetImages;
            //    try
            //    {
            //        jsonGadgetImages = await page.EvaluateExpressionAsync<string>(
            //        @"
            //            (function () {
            //                // Lấy tất cả các thẻ con bên trong theo thứ tự xuất hiện
            //                const elements = document.querySelectorAll('div.swiper div.swiper-wrapper div.swiper-slide.Slideshow_swiperSlideCustom___6FRx img');
            //                if (elements) {

            //                    // Tạo một mảng để lưu nội dung các thẻ
            //                    const imgList = [];

            //                    // Lặp qua từng thẻ và lấy nội dung hoặc thuộc tính tương ứng
            //                    for (let element of elements) {
            //                        const gadgetImage = {
            //                            imageUrl: element.getAttribute('src')
            //                        }
            //                        if (gadgetImage.imageUrl) {
            //                            imgList.push(gadgetImage);
            //                        }
            //                    }

            //                    return JSON.stringify(imgList); // Trả về mảng chứa nội dung của các thẻ
            //                } else {
            //                    console.log('Không tìm thấy thẻ div');
            //                    return JSON.stringify([]);
            //                }
            //            })()
            //        ");
            //        List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(jsonGadgetImages)!;
            //        if (gadgetImages.Count > 0)
            //        {
            //            gadgetDetail.GadgetImages = gadgetImages;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //    //Click view thông số kỹ thuật detail
            //    moreDataToLoad = true;
            //    while (moreDataToLoad)
            //    {
            //        try
            //        {
            //            var button = await page.WaitForSelectorAsync("button.flex.items-center.text-blue-blue-7.b2-medium", new WaitForSelectorOptions { Timeout = 2000 });

            //            if (button != null)
            //            {
            //                Console.WriteLine("Xem chi tiết thông số kỹ thuật");
            //                var isButtonVisible = await button.IsVisibleAsync();
            //                if (isButtonVisible)
            //                {
            //                    await button.PressAsync("Enter");
            //                    await Task.Delay(2000);
            //                    try
            //                    {
            //                        await page.WaitForSelectorAsync("div.Sheet_sheet__59S7D", new WaitForSelectorOptions { Timeout = 2000 });
            //                        moreDataToLoad = false;
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine("popup catch " + ex.Message);
            //                        moreDataToLoad = true;
            //                    }
            //                }
            //                else
            //                {
            //                    moreDataToLoad = false;
            //                }
            //            }
            //            else
            //            {
            //                moreDataToLoad = false;
            //            }
            //        }
            //        catch (PuppeteerException ex)
            //        {
            //            Console.WriteLine("catch " + ex.Message);
            //            moreDataToLoad = false;
            //        }
            //    }

            //    //Get gadget specifications
            //    string jsonGadgetSpecifications;
            //    try
            //    {
            //        jsonGadgetSpecifications = await page.EvaluateExpressionAsync<string>(
            //        @"
            //            (function () {
            //                const specifications = [];

            //                const specificationElements = document.querySelectorAll('div.Sheet_sheet__59S7D div.Sheet_body__VKc95.active div.tab-content.flex.flex-col.pt-5');

            //                for (let i = 0; i < specificationElements.length; i++) {
            //                    const specificationElement = specificationElements[i].querySelector('div.mb-1.flex.items-center.gap-2.text-textOnWhitePrimary.b2-semibold span');
            //                    const specificationName = specificationElement?.textContent?.trim();
            //                    if (specificationName == null || specificationName === '') {
            //                        continue;
            //                    }
            //                    const specification = {
            //                        name: specificationName,
            //                        specificationKeys: []
            //                    };
            //                    const rowsElement = specificationElements[i].querySelectorAll('div.flex.gap-2.border-b');
            //                    for (let j = 0; j < rowsElement.length; j++) {
            //                        const specificationKey = {
            //                            name: null,
            //                            specificationValues: []
            //                        }
            //                        const specificationKeyName = rowsElement[j].querySelector('div.text-textOnWhiteSecondary.b2-regular span').textContent.trim();
            //                        if (specificationKeyName) {
            //                            specificationKey.name = specificationKeyName;
            //                        } else {
            //                            continue;
            //                        }

            //                        // Kiểm tra xem value là span hay p
            //                        const spanValue = rowsElement[j].querySelector('span.flex-1.text-textOnWhitePrimary.b2-regular');
            //                        const pValues = rowsElement[j].querySelectorAll('div.flex.flex-1.flex-col p');

            //                        const specificationValue = {
            //                            value: null,
            //                        }

            //                        if (spanValue) {
            //                            // Nếu chỉ có 1 value (thẻ span)
            //                            specificationValue.value = spanValue.textContent.trim();
            //                            specificationKey.specificationValues.push(specificationValue);
            //                        } else if (pValues.length > 0) {
            //                            // Nếu có nhiều value (các thẻ p)
            //                            for (let k = 0; k < pValues.length; k++) {
            //                                const specificationValue = {
            //                                    value: null,
            //                                }
            //                                specificationValue.value = pValues[k].textContent.trim();
            //                                specificationKey.specificationValues.push(specificationValue);
            //                            }
            //                        } else {
            //                            continue;
            //                        }

            //                        specification.specificationKeys.push(specificationKey);
            //                    }

            //                    specifications.push(specification);
            //                }

            //                return JSON.stringify(specifications);
            //            })()
            //        ");
            //        List<Specification> gadgetSpecifications = JsonConvert.DeserializeObject<List<Specification>>(jsonGadgetSpecifications)!;
            //        if (gadgetSpecifications.Count > 0)
            //        {
            //            gadgetDetail.Specifications = gadgetSpecifications;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }

            //    listGadgets.Add(gadgetDetail);

            //    Console.WriteLine(JsonConvert.SerializeObject(gadgetDetail));
            //    Console.WriteLine("add success");

            //    //Click nút close popup
            //    moreDataToLoad = true;
            //    while (moreDataToLoad && j < numberOfRelatedGadget - 1)
            //    {
            //        try
            //        {
            //            var clsButton = await page.WaitForSelectorAsync("svg.Sheet_icon__RnybF", new WaitForSelectorOptions { Timeout = 2000 });

            //            if (clsButton != null)
            //            {
            //                var isButtonVisible = await clsButton.IsVisibleAsync();
            //                if (isButtonVisible)
            //                {
            //                    Console.WriteLine("clsButton visible");
            //                    await clsButton.ClickAsync();
            //                    await Task.Delay(2000);
            //                    try
            //                    {
            //                        await page.WaitForSelectorAsync("div.Sheet_sheet__59S7D", new WaitForSelectorOptions { Timeout = 2000 });
            //                        moreDataToLoad = true;
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine("popup catch " + ex.Message);
            //                        Console.WriteLine("Đã đón popup");
            //                        moreDataToLoad = false;
            //                    }
            //                }
            //                else
            //                {
            //                    Console.WriteLine("clsButton invisible");
            //                    moreDataToLoad = false;
            //                }
            //            }
            //            else
            //            {
            //                moreDataToLoad = false;
            //            }
            //        }
            //        catch (PuppeteerException ex)
            //        {
            //            Console.WriteLine("clsButton catch " + ex.Message);
            //            moreDataToLoad = false;
            //        }
            //    }

            //    //Click next gadget detail
            //    if (j < numberOfRelatedGadget - 1)
            //    {
            //        bool isNotClicked = true;
            //        while (isNotClicked)
            //        {
            //            try
            //            {
            //                // Lấy tất cả các nút có class 'Selection_button__vX7ZX'
            //                var buttons = await page.QuerySelectorAllAsync("button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_buttonSelect__7lW_h.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy, button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy");

            //                await buttons[j + 1].PressAsync("Enter");
            //                await Task.Delay(2000);
            //                try
            //                {
            //                    isNotClicked = !await page.EvaluateExpressionAsync<bool>(
            //                    $@"
            //                        (function () {{
            //                            const listBtn = document.querySelectorAll('button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_buttonSelect__7lW_h.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy, button.Selection_button__vX7ZX.Selection_onlyTextContainer__OQkpW.Selection_btnLarge__ErN2N.Selection_largeOnlyContentContainer__Epac2.Selection_selectionRed__r8aEy');
            //                            const isClicked = listBtn[{j + 1}].classList.contains('Selection_buttonSelect__7lW_h');
            //                            return isClicked;
            //                        }})()
            //                    ");
            //                    if (isNotClicked)
            //                    {
            //                        Console.WriteLine($"Nút {j + 1} chưa được nhấn");
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine($"Đã nhấn vào nút {j + 1}");
            //                        await Task.Delay(2000);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                    isNotClicked = true;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            //                isNotClicked = true;
            //            }
            //        }
            //    }
            //}
        }
        await browser.CloseAsync();

        return listGadgets;
    }
}
