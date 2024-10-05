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

public class ScrapeTGDDDataService(IOptions<ScrapeDataSettings> scrapeDataSettings, AppDbContext context, GadgetScrapeDataService gadgetScrapeDataService)
{
    private readonly ScrapeDataSettings _scrapeDataSettings = scrapeDataSettings.Value;
    private readonly GadgetScrapeDataService _gadgetScrapeDataService = gadgetScrapeDataService;
    public async Task ScrapeTGDDGadget()
    {
        var tgddShop = await context.Shops
            .FirstOrDefaultAsync(s => s.Name == "Thế Giới Di Động");
        if (tgddShop == null)
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
                            string phoneUrl = _scrapeDataSettings.TGDD + "dtdd";
                            switch (brand.Name)
                            {
                                case "Samsung":
                                    phoneUrl += "#c=42&m=2&o=13&pi=0";
                                    break;
                                case "Apple":
                                    phoneUrl += "#c=42&m=80&o=13&pi=0";
                                    break;
                                case "Oppo":
                                    phoneUrl += "#c=42&m=1971&o=13&pi=0";
                                    break;
                                case "Xiaomi":
                                    phoneUrl += "#c=42&m=2235&o=13&pi=0";
                                    break;
                                case "Vivo":
                                    phoneUrl += "#c=42&m=2236&o=13&pi=0";
                                    break;
                                case "Realme":
                                    phoneUrl += "#c=42&m=17201&o=13&pi=0";
                                    break;
                                case "Honor":
                                    phoneUrl += "#c=42&m=2283&o=13&pi=0";
                                    break;
                                case "TCL":
                                    phoneUrl += "#c=42&m=1541&o=13&pi=0";
                                    break;
                                case "Tecno":
                                    phoneUrl += "#c=42&m=36747&o=13&pi=0";
                                    break;
                                case "Nokia":
                                    phoneUrl += "#c=42&m=1&o=13&pi=0";
                                    break;
                                case "Masstel":
                                    phoneUrl += "#c=42&m=4832&o=13&pi=0";
                                    break;
                                case "Mobell":
                                    phoneUrl += "#c=42&m=19&o=13&pi=0";
                                    break;
                                case "Itel":
                                    phoneUrl += "#c=42&m=5332&o=13&pi=0";
                                    break;
                                case "Viettel":
                                    phoneUrl += "#c=42&m=1711&o=13&pi=0";
                                    break;
                                case "Benco":
                                    phoneUrl += "#c=42&m=38897&o=13&pi=0";
                                    break;
                                default:
                                    continue;
                            }

                            List<Gadget> listGadgets = new List<Gadget>()!;
                            try
                            {
                                listGadgets = await ScrapeGadgetByBrand(phoneUrl, brand.Name);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Điện thoại {brand.Name}: {ex}");
                                continue;
                            }
                            await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, tgddShop, context);
                        }
                        break;
                    case "Laptop":
                        foreach (var brand in cateWithBrands.Brands)
                        {
                            string laptopUrl = _scrapeDataSettings.TGDD + "laptop";
                            switch (brand.Name)
                            {
                                case "Hp":
                                    laptopUrl += "#c=44&m=122&o=13&pi=0";
                                    break;
                                case "Asus":
                                    laptopUrl += "#c=44&m=128&o=13&pi=0";
                                    break;
                                case "Acer":
                                    laptopUrl += "#c=44&m=119&o=13&pi=0";
                                    break;
                                case "Lenovo":
                                    laptopUrl += "#c=44&m=120&o=13&pi=0";
                                    break;
                                case "Dell":
                                    laptopUrl += "#c=44&m=118&o=13&pi=0";
                                    break;
                                case "MSI":
                                    laptopUrl += "#c=44&m=133&o=13&pi=0";
                                    break;
                                case "Apple":
                                    laptopUrl += "#c=44&m=203&o=13&pi=0";
                                    break;
                                case "Samsung":
                                    laptopUrl += "#c=44&m=646&o=13&pi=0";
                                    break;
                                default:
                                    continue;
                            }

                            List<Gadget> listGadgets = new List<Gadget>()!;
                            try
                            {
                                listGadgets = await ScrapeGadgetByBrand(laptopUrl, brand.Name);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Laptop {brand.Name}: {ex}");
                                continue;
                            }
                            await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, tgddShop, context);
                        }
                        break;
                    case "Thiết bị âm thanh":
                        foreach (var brand in cateWithBrands.Brands)
                        {
                            string soundUrl = _scrapeDataSettings.TGDD;
                            bool isCaseNormal = true;

                            //Case brand có tai nghe hoặc loa
                            switch (brand.Name)
                            {
                                case "Soul":
                                    soundUrl += "tai-nghe#c=54&m=37540&o=13&pi=0";
                                    break;
                                case "Havit":
                                    soundUrl += "tai-nghe#c=54&m=36948&o=13&pi=0";
                                    break;
                                case "Beats":
                                    soundUrl += "tai-nghe#c=54&m=8090&o=13&pi=0";
                                    break;
                                case "Soundcore":
                                    soundUrl += "tai-nghe#c=54&m=38905&o=13&pi=0";
                                    break;
                                case "Zadez":
                                    soundUrl += "tai-nghe#c=54&m=37633&o=13&pi=0";
                                    break;
                                case "HyperX":
                                    soundUrl += "tai-nghe#c=54&m=38595&o=13&pi=0";
                                    break;
                                case "Oppo":
                                    soundUrl += "tai-nghe#c=54&m=24552&o=13&pi=0";
                                    break;
                                case "Shokz":
                                    soundUrl += "tai-nghe#c=54&m=38243&o=13&pi=0";
                                    break;
                                case "Baseus":
                                    soundUrl += "tai-nghe#c=54&m=37013&o=13&pi=0";
                                    break;
                                case "Soundpeats":
                                    soundUrl += "tai-nghe#c=54&m=24085&o=13&pi=0";
                                    break;
                                case "Asus":
                                    soundUrl += "tai-nghe#c=54&m=9299&o=13&pi=0";
                                    break;
                                case "Realme":
                                    soundUrl += "tai-nghe#c=54&m=20723&o=13&pi=0";
                                    break;
                                case "Apple":
                                    soundUrl += "tai-nghe#c=54&m=2660&o=13&pi=0";
                                    break;
                                case "Denon":
                                    soundUrl += "tai-nghe#c=54&m=15238&o=13&pi=0";
                                    break;
                                case "Harman Kardon":
                                    soundUrl += "loa-laptop#c=2162&m=20479&o=13&pi=0";
                                    break;
                                case "Microlab":
                                    soundUrl += "loa-laptop#c=2162&m=19768&o=13&pi=0";
                                    break;
                                case "Soundmax":
                                    soundUrl += "loa-laptop#c=2162&m=38725&o=13&pi=0";
                                    break;
                                case "LG":
                                    soundUrl += "loa-laptop#c=2162&m=2197&o=13&pi=0";
                                    break;
                                case "Fenda":
                                    soundUrl += "loa-laptop#c=2162&m=19758&o=13&pi=0";
                                    break;
                                case "Alpha Works":
                                    soundUrl += "loa-laptop#c=2162&m=38245&o=13&pi=0";
                                    break;
                                case "Klipsch":
                                    soundUrl += "loa-laptop#c=2162&m=24025&o=13&pi=0";
                                    break;
                                case "Enkor":
                                    soundUrl += "loa-laptop#c=2162&m=19759&o=13&pi=0";
                                    break;
                                case "Nanomax":
                                    soundUrl += "loa-laptop#c=2162&m=14901&o=13&pi=0";
                                    break;
                                case "Zenbos":
                                    soundUrl += "loa-laptop#c=2162&m=19070&o=13&pi=0";
                                    break;
                                case "Jammy":
                                    soundUrl += "loa-laptop#c=2162&m=19742&o=13&pi=0";
                                    break;
                                case "Sumico":
                                    soundUrl += "loa-laptop#c=2162&m=24319&o=13&pi=0";
                                    break;
                                case "Paramax":
                                    soundUrl += "loa-laptop#c=2162&m=31376&o=13&pi=0";
                                    break;
                                case "Dalton":
                                    soundUrl += "loa-laptop#c=2162&m=11925&o=13&pi=0";
                                    break;
                                case "Birici":
                                    soundUrl += "loa-laptop#c=2162&m=17255&o=13&pi=0";
                                    break;
                                case "Mobell":
                                    soundUrl += "loa-laptop#c=2162&m=16823&o=13&pi=0";
                                    break;
                                case "Pasion":
                                    soundUrl += "loa-laptop#c=2162&m=36790&o=13&pi=0";
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
                                    listGadgets = await ScrapeGadgetByBrand(soundUrl, brand.Name);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Thiết bị âm thanh {brand.Name}: {ex}");
                                    continue;
                                }

                                await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, tgddShop, context);
                                continue;
                            }
                            List<string> listSoundUrls = new List<string>()!;
                            string earPhoneUrl = soundUrl;
                            string loudspeakerUrl = soundUrl;

                            //Case brand vừa có tai nghe vừa có loa
                            switch (brand.Name)
                            {
                                case "JBL":
                                    earPhoneUrl += "tai-nghe#c=54&m=5392&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=1176&o=13&pi=0";
                                    break;
                                case "AVA":
                                    earPhoneUrl += "tai-nghe#c=54&m=2987&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=1688&o=13&pi=0";
                                    break;
                                case "Rezo":
                                    earPhoneUrl += "tai-nghe#c=54&m=29872&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=36826&o=13&pi=0";
                                    break;
                                case "Sony":
                                    earPhoneUrl += "tai-nghe#c=54&m=1842&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=2193&o=13&pi=0";
                                    break;
                                case "Marshall":
                                    earPhoneUrl += "tai-nghe#c=54&m=14583&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=1454&o=13&pi=0";
                                    break;
                                case "Sounarc":
                                    earPhoneUrl += "tai-nghe#c=54&m=38726&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=38719&o=13&pi=0";
                                    break;
                                case "Monster":
                                    earPhoneUrl += "tai-nghe#c=54&m=36400&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=36396&o=13&pi=0";
                                    break;
                                case "Xiaomi":
                                    earPhoneUrl += "tai-nghe#c=54&m=7710&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=38567&o=13&pi=0";
                                    break;
                                case "Mozard":
                                    earPhoneUrl += "tai-nghe#c=54&m=8157&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=19762&o=13&pi=0";
                                    break;
                                case "Samsung":
                                    earPhoneUrl += "tai-nghe#c=54&m=2391&o=13&pi=0";
                                    loudspeakerUrl += "loa-laptop#c=2162&m=2196&o=13&pi=0";
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
                                        listGadgets = await ScrapeGadgetByBrand(urlItem, brand.Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Có lỗi xảy ra trong quá trình scrape Thiết bị âm thanh {brand.Name}: {ex}");
                                        continue;
                                    }

                                    await _gadgetScrapeDataService.AddGadgetToDB(listGadgets, brand, cateWithBrands, tgddShop, context);
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
    private async Task<List<Gadget>> ScrapeGadgetByBrand(string url, string brandName)
    {
        bool isApple;
        if (brandName == "Apple")
        {
            isApple = true;
        }
        else
        {
            isApple = false;
        }
        string defaultUrl = _gadgetScrapeDataService.CutUrl(url);
        // Tải xuống Chromium nếu chưa có
        await new BrowserFetcher().DownloadAsync();

        // Khởi tạo trình duyệt
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true // Chạy ở chế độ headless để không hiển thị giao diện
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
                var button = await page.WaitForSelectorAsync("div.view-more a", new WaitForSelectorOptions { Timeout = 2000 });

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
                moreDataToLoad = false;
            }
        }

        //Scrape Price, ThumbnailUrl, NavigationUrl
        string jsonGadgets = await page.EvaluateExpressionAsync<string>(
            @"
                (function () {
                    const listGadgetsLi = Array.from(document.querySelectorAll('ul.listproduct li'));
                    const newGadgetItemJsonResponses = [];
                    for (let i = 0; i < listGadgetsLi.length; i++) {
                        const li = listGadgetsLi[i];
                        const GadgetItemJsonResponse = {
                            price: parseInt(li.getAttribute('data-price')),
                            thumbnailUrl: null,
                            navigationUrl: null,
                        };

                        const aElement = li.querySelector('a');
                        if (aElement) {
                            GadgetItemJsonResponse.navigationUrl = aElement.getAttribute('href');
                        }

                        const imgElement = li.querySelector('div.item-img img');
                        if (imgElement) {
                            GadgetItemJsonResponse.thumbnailUrl = imgElement.getAttribute('src');
                        }

                        const imgElementSpecial = Array.from(li.querySelectorAll('div.item-img img[class*=""lazyload""], img[class*=""lazyloading""], img[class*=""lazyloaded""]'));
                        if (imgElementSpecial.length != 0) {
                            GadgetItemJsonResponse.thumbnailUrl = imgElementSpecial[0].getAttribute('data-src');
                        }

                        if (GadgetItemJsonResponse.price || GadgetItemJsonResponse.thumbnailUrl) {
                            newGadgetItemJsonResponses.push(GadgetItemJsonResponse);
                        }
                    }
                    return JSON.stringify(newGadgetItemJsonResponses);
                })()
            ");
        List<GadgetItemJsonResponse> GadgetItemJsonResponses = JsonConvert.DeserializeObject<List<GadgetItemJsonResponse>>(jsonGadgets)!;

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

            //Scrape Specifications, SpecificationKeys, SpecificationValues
            Gadget gadgetDetail;
            string jsonGadgetDetail;
            try
            {
                jsonGadgetDetail = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const gadgetNameElement = document.querySelector('div.product-name h1');
                            const gadgetName = gadgetNameElement ? gadgetNameElement.innerHTML : 'N/A';

                            const listSpecifications = Array.from(document.querySelectorAll('div.box-specifi'));
                            const listSpecificationKeysUl = Array.from(document.querySelectorAll('ul.text-specifi'));

                            const gadget = {
                                name: gadgetName,
                                specifications: [],
                            }

                            for (let i = 0; i < listSpecifications.length; i++) {
                                const item = listSpecifications[i];
                                const aElement = item.querySelector('a');
                                const specification = {
                                    name: item.querySelector('h3').innerHTML.trim(),
                                    specificationKeys: [],
                                }
                                const specificationKey = listSpecificationKeysUl[i];
                                const listSpecificationRowLi = Array.from(specificationKey.querySelectorAll('li'));

                                for (let j = 0; j < listSpecificationRowLi.length; j++) {
                                    const listSpecificationColumnAside = Array.from(listSpecificationRowLi[j].querySelectorAll('aside'));
                                    const specificationKey = {
                                        name: '',
                                        specificationValues: [],
                                    }
                                    for (let k = 0; k < listSpecificationColumnAside.length; k++) {
                                        if (k == 0) { //TH SpecificationKey
                                            const asideChild = listSpecificationColumnAside[k].querySelector('a, span'); // Kiểm tra nếu có <a> hoặc <span>
                                            if (asideChild) {
                                                specificationKey.name = asideChild.textContent.trim();  // Lấy giá trị của <a> hoặc <span>
                                            } else {
                                                specificationKey.name = listSpecificationColumnAside[k].textContent.trim();  // Lấy giá trị text nếu không có <a>/<span>
                                            }
                                        } else if (k == 1) { //TH SpecificationValue
                                            const listSpecificationValues = Array.from(listSpecificationColumnAside[k].querySelectorAll('a, span'));
                                            for (let l = 0; l < listSpecificationValues.length; l++) {
                                                const specificationValue = {
                                                    value: listSpecificationValues[l].textContent.trim()
                                                }
                                                specificationKey.specificationValues.push(specificationValue);
                                            }
                                        }
                                    }
                                    specification.specificationKeys.push(specificationKey);
                                }

                                gadget.specifications.push(specification);
                            }
                            return JSON.stringify(gadget);
                        })()
                    ");
                gadgetDetail = JsonConvert.DeserializeObject<Gadget>(jsonGadgetDetail)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                jsonGadgetDetail = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const gadgetNameElement = document.querySelector('div.product-name h1');
                            const gadgetName = gadgetNameElement ? gadgetNameElement.innerHTML : 'N/A';

                            const specificationKeyUlElement = document.querySelector('ul.text-specifi');

                            const gadget = {
                                name: gadgetName,
                                specificationKeys: [],
                            }

                            const listSpecificationRowLi = Array.from(specificationKeyUlElement.querySelectorAll('li'));

                            for (let j = 0; j < listSpecificationRowLi.length; j++) {
                                const listSpecificationColumnAside = Array.from(listSpecificationRowLi[j].querySelectorAll('aside'));
                                const specificationKey = {
                                    name: '',
                                    specificationValues: [],
                                }
                                for (let k = 0; k < listSpecificationColumnAside.length; k++) {
                                    if (k == 0) { //TH SpecificationKey
                                        const asideChild = listSpecificationColumnAside[k].querySelector('a, span'); // Kiểm tra nếu có <a> hoặc <span>
                                        if (asideChild) {
                                            specificationKey.name = asideChild.textContent.trim();  // Lấy giá trị của <a> hoặc <span>
                                        } else {
                                            specificationKey.name = listSpecificationColumnAside[k].textContent.trim();  // Lấy giá trị text nếu không có <a>/<span>
                                        }
                                    } else if (k == 1) { //TH SpecificationValue
                                        const listSpecificationValues = Array.from(listSpecificationColumnAside[k].querySelectorAll('a, span'));
                                        for (let l = 0; l < listSpecificationValues.length; l++) {
                                            const specificationValue = {
                                                value: listSpecificationValues[l].textContent.trim()
                                            }
                                            specificationKey.specificationValues.push(specificationValue);
                                        }
                                    }
                                }
                                gadget.specificationKeys.push(specificationKey);
                            }
                            return JSON.stringify(gadget);
                        })()
                    ");
                gadgetDetail = JsonConvert.DeserializeObject<Gadget>(jsonGadgetDetail)!;
            }
            Console.WriteLine("ktra: " + gadgetDetail.Name);

            //TH các điện thoại có kiểu style đặc biệt cần phải vào lấy rõ hơn
            if (gadgetDetail.Name == "N/A" && !isApple)
            {
                string specialGadgetNames = await page.EvaluateExpressionAsync<string>(
                @"
                    (function () {
                        const listSpecialGadgetsElement = Array.from(document.querySelectorAll('section.wrapper.config.page1:not(.hide) div.tab a'));
                        const listSpecialGadgets = [];
                        for (let i = 0; i < listSpecialGadgetsElement.length; i++) {
                            const gadgetName = listSpecialGadgetsElement[i].textContent.trim();
                            listSpecialGadgets.push(gadgetName);
                        }
                        return JSON.stringify(listSpecialGadgets);
                    })()
                ");
                List<string> specialGadgetItemJsonResponses = JsonConvert.DeserializeObject<List<string>>(specialGadgetNames)!;
                if (specialGadgetItemJsonResponses.Count == 0)
                {
                    Gadget specialGadget = new Gadget()!;
                    specialGadget.ThumbnailUrl = GadgetItemJsonResponses[i].ThumbnailUrl;
                    specialGadget.Url = page.Url;

                    //Get gadget price
                    try
                    {
                        string priceWithSpecificationDetail = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const gadgetSpecificationNameElement = document.querySelector('section.wrapper.config.page1:not(.hide) div.item.cf-left b b');
                                if (gadgetSpecificationNameElement) {
                                    return gadgetSpecificationNameElement.innerHTML;
                                }
                                return '0đ';
                            })()
                        ");
                        specialGadget.Price = _gadgetScrapeDataService.ConvertPriceToInt(priceWithSpecificationDetail);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Scrape Gadget Images
                    string listGadgetImages = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const listGadgetImages = [];

                            const listOutstandingImages = Array.from(document.querySelectorAll('section.wrapper.page1.key div.container img'));
                            for (let i = 0; i < listOutstandingImages.length; i++) {
                                const gadgetImage = {
                                    imageUrl: ''
                                }
                                const imgSrc = listOutstandingImages[i].getAttribute('src');
                                if (imgSrc) {
                                    gadgetImage.imageUrl = imgSrc;
                                    listGadgetImages.push(gadgetImage);
                                }
                            }

                            return JSON.stringify(listGadgetImages);
                        })()
                    ");
                    List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(listGadgetImages)!;
                    specialGadget.GadgetImages = gadgetImages;

                    //Get gadget description
                    var backgroundImageUrl = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const divElement = document.querySelector('section#feature.wrapper.page1.kvs.c1.video div.bg');

                            if (divElement) {
                                const backgroundImage = divElement.getAttribute('data-background')
                                if (backgroundImage) {
                                    // Loại bỏ ký tự không cần thiết như &quot; hoặc ""
                                    return backgroundImage.replace(/&quot;|['\""]/g, '');
                                } else {
                                    const backgroundUrl = divElement.style.backgroundImage;
                                    // Sử dụng regex để trích xuất URL từ background-image
                                    const urlMatch = backgroundUrl.match(/soundUrl\(\s*['\""]?(.*?)['\""]?\s*\)/);
                                    return urlMatch ? urlMatch[1] : 'khong match soundUrl';
                                }
                            }
                            return 'khong thay soundUrl';
                        })()
                    ");
                    GadgetDescription gadgetDescription = new GadgetDescription()!;
                    gadgetDescription.Type = GadgetDescriptionType.Image;
                    gadgetDescription.Index = 0;
                    gadgetDescription.Value = backgroundImageUrl;
                    List<GadgetDescription> gadgetDescriptions = new List<GadgetDescription>()!;
                    gadgetDescriptions.Add(gadgetDescription);
                    specialGadget.GadgetDescriptions = gadgetDescriptions;

                    //Click view detail info
                    moreDataToLoad = true;
                    while (moreDataToLoad)
                    {
                        try
                        {
                            var buttonDetail = await page.WaitForSelectorAsync("section.wrapper.config.page1:not(.hide) a.viewmore", new WaitForSelectorOptions { Timeout = 2000 });

                            if (buttonDetail != null)
                            {
                                var isButtonDetailVisible = await buttonDetail.IsVisibleAsync();
                                if (isButtonDetailVisible)
                                {
                                    Console.WriteLine("buttonDetail visible");
                                    await buttonDetail.ClickAsync();
                                    await Task.Delay(2000);
                                    try
                                    {
                                        var popup = await page.WaitForSelectorAsync("div#colorbox", new WaitForSelectorOptions { Timeout = 2000 });
                                        if (popup != null)
                                        {
                                            var isPopupVisible = await popup.IsVisibleAsync();
                                            if (isPopupVisible)
                                            {
                                                Console.WriteLine("popup visible");
                                                moreDataToLoad = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("popup invisible");
                                            }
                                        }
                                    }
                                    catch (PuppeteerException ex)
                                    {
                                        Console.WriteLine("popup catch " + ex.Message);
                                        moreDataToLoad = true;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("buttonDetail invisible");
                                    moreDataToLoad = false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("buttonDetail null");
                                moreDataToLoad = false;
                            }
                        }
                        catch (PuppeteerException ex)
                        {
                            Console.WriteLine("buttonDetail catch " + ex.Message);
                            moreDataToLoad = false;
                        }
                    }

                    //Get gadget detail name
                    string specialGadgetName = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const gadgetH4Element = document.querySelector('#colorbox h4');
                            return gadgetH4Element.textContent.trim();
                        })()
                    ");
                    string specialGadgetNameResult = specialGadgetName.Replace("Thông số kỹ thuật chi tiết ", "Điện thoại ");
                    specialGadget.Name = specialGadgetNameResult;

                    //Scrape thông số kỹ thuật
                    try
                    {
                        string specificationDetail = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const gadget = {
                                    name: '',
                                    specifications: [],
                                }

                                const liElements = document.querySelectorAll('ul.parameterfull li');
                                const result = [];
                                let specification = null;

                                // Duyệt qua từng thẻ li
                                liElements.forEach(li => {
                                    // Nếu li không có class => là cha
                                    if (li.classList.length === 0) {
                                        // Tạo đối tượng cha mới và thêm vào danh sách specifications
                                        specification = {
                                            name: li.querySelector('label').textContent.trim(),   // thẻ li không có class
                                            specificationKeys: []  // khởi tạo mảng chứa các li con
                                        };
                                        gadget.specifications.push(specification);
                                    }
                                    // Nếu li có class => là con của li cha trước đó
                                    else if (specification) {
                                        const keySpan = li.querySelector('span'); // Lấy thẻ <span> đầu tiên
                                        const div = li.querySelector('div');      // Lấy thẻ <div> kế tiếp

                                        if (keySpan && div) {
                                            const specificationRow = {
                                                name: keySpan.innerText.trim(),
                                                specificationValues: []
                                            }                                   // Mảng chứa các value

                                            // Kiểm tra nếu trong <div> có các thẻ <a> hoặc <span> thì thêm vào mảng values
                                            div.querySelectorAll('a, span, p').forEach(element => {
                                                const specificationValue = {
                                                    value: element.innerText.trim()
                                                }
                                                specificationRow.specificationValues.push(specificationValue);
                                            });

                                            specification.specificationKeys.push(specificationRow); // Thêm thẻ li có class vào danh sách con của cha hiện tại
                                        }
                                    }
                                });
                                return JSON.stringify(gadget);
                            })()
                        ");
                        Gadget gadgetSpecificationDetail = JsonConvert.DeserializeObject<Gadget>(specificationDetail)!;
                        specialGadget.Specifications = gadgetSpecificationDetail.Specifications;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }

                    listGadgets.Add(specialGadget);
                    Console.WriteLine("add success");
                }

                for (int k = 0; k < specialGadgetItemJsonResponses.Count; k++)
                {
                    Gadget specialGadget = new Gadget()!;
                    specialGadget.Name = "Điện thoại " + brandName + " " + specialGadgetItemJsonResponses[k];
                    specialGadget.ThumbnailUrl = GadgetItemJsonResponses[i].ThumbnailUrl;
                    specialGadget.Url = page.Url;

                    //Get gadget price
                    try
                    {
                        string priceWithSpecificationDetail = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const gadgetSpecificationNameElement = document.querySelector('section.wrapper.config.page1:not(.hide) div.item.cf-left b b');
                                if (gadgetSpecificationNameElement) {
                                    return gadgetSpecificationNameElement.innerHTML;
                                }
                                return '0đ';
                            })()
                        ");
                        specialGadget.Price = _gadgetScrapeDataService.ConvertPriceToInt(priceWithSpecificationDetail);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //Scrape Gadget Images
                    string listGadgetImages = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const listGadgetImages = [];

                            const listOutstandingImages = Array.from(document.querySelectorAll('section.wrapper.page1.key div.container img'));
                            for (let i = 0; i < listOutstandingImages.length; i++) {
                                const gadgetImage = {
                                    imageUrl: ''
                                }
                                const imgSrc = listOutstandingImages[i].getAttribute('src');
                                if (imgSrc) {
                                    gadgetImage.imageUrl = imgSrc;
                                    listGadgetImages.push(gadgetImage);
                                }
                            }

                            return JSON.stringify(listGadgetImages);
                        })()
                    ");
                    List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(listGadgetImages)!;
                    specialGadget.GadgetImages = gadgetImages;

                    //Get gadget description
                    var backgroundImageUrl = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const divElement = document.querySelector('section#feature.wrapper.page1.kvs.c1.video div.bg');

                            if (divElement) {
                                const backgroundImage = divElement.getAttribute('data-background')
                                if (backgroundImage) {
                                    // Loại bỏ ký tự không cần thiết như &quot; hoặc ""
                                    return backgroundImage.replace(/&quot;|['\""]/g, '');
                                } else {
                                    const backgroundUrl = divElement.style.backgroundImage;
                                    // Sử dụng regex để trích xuất URL từ background-image
                                    const urlMatch = backgroundUrl.match(/soundUrl\(\s*['\""]?(.*?)['\""]?\s*\)/);
                                    return urlMatch ? urlMatch[1] : 'khong match soundUrl';
                                }
                            }
                            return 'khong thay soundUrl';
                        })()
                    ");
                    GadgetDescription gadgetDescription = new GadgetDescription()!;
                    gadgetDescription.Type = GadgetDescriptionType.Image;
                    gadgetDescription.Index = 0;
                    gadgetDescription.Value = backgroundImageUrl;
                    List<GadgetDescription> gadgetDescriptions = new List<GadgetDescription>()!;
                    gadgetDescriptions.Add(gadgetDescription);
                    specialGadget.GadgetDescriptions = gadgetDescriptions;

                    //Click view detail info
                    moreDataToLoad = true;
                    while (moreDataToLoad)
                    {
                        try
                        {
                            var buttonDetail = await page.WaitForSelectorAsync("section.wrapper.config.page1:not(.hide) a.viewmore", new WaitForSelectorOptions { Timeout = 2000 });

                            if (buttonDetail != null)
                            {
                                var isButtonDetailVisible = await buttonDetail.IsVisibleAsync();
                                if (isButtonDetailVisible)
                                {
                                    Console.WriteLine("buttonDetail visible");
                                    await buttonDetail.ClickAsync();
                                    await Task.Delay(2000);
                                    try
                                    {
                                        var popup = await page.WaitForSelectorAsync("div#colorbox", new WaitForSelectorOptions { Timeout = 2000 });
                                        if (popup != null)
                                        {
                                            var isPopupVisible = await popup.IsVisibleAsync();
                                            if (isPopupVisible)
                                            {
                                                Console.WriteLine("popup visible");
                                                moreDataToLoad = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("popup invisible");
                                            }
                                        }
                                    }
                                    catch (PuppeteerException ex)
                                    {
                                        Console.WriteLine("popup catch " + ex.Message);
                                        moreDataToLoad = true;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("buttonDetail invisible");
                                    moreDataToLoad = false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("buttonDetail null");
                                moreDataToLoad = false;
                            }
                        }
                        catch (PuppeteerException ex)
                        {
                            Console.WriteLine("buttonDetail catch " + ex.Message);
                            moreDataToLoad = false;
                        }
                    }

                    //Scrape thông số kỹ thuật
                    try
                    {
                        string specificationDetail = await page.EvaluateExpressionAsync<string>(
                        @"
                            (function () {
                                const gadget = {
                                    name: '',
                                    specifications: [],
                                }

                                const liElements = document.querySelectorAll('ul.parameterfull li');
                                const result = [];
                                let specification = null;

                                // Duyệt qua từng thẻ li
                                liElements.forEach(li => {
                                    // Nếu li không có class => là cha
                                    if (li.classList.length === 0) {
                                        // Tạo đối tượng cha mới và thêm vào danh sách specifications
                                        specification = {
                                            name: li.querySelector('label').textContent.trim(),   // thẻ li không có class
                                            specificationKeys: []  // khởi tạo mảng chứa các li con
                                        };
                                        gadget.specifications.push(specification);
                                    }
                                    // Nếu li có class => là con của li cha trước đó
                                    else if (specification) {
                                        const keySpan = li.querySelector('span'); // Lấy thẻ <span> đầu tiên
                                        const div = li.querySelector('div');      // Lấy thẻ <div> kế tiếp

                                        if (keySpan && div) {
                                            const specificationRow = {
                                                name: keySpan.innerText.trim(),
                                                specificationValues: []
                                            }                                   // Mảng chứa các value

                                            // Kiểm tra nếu trong <div> có các thẻ <a> hoặc <span> thì thêm vào mảng values
                                            div.querySelectorAll('a, span, p').forEach(element => {
                                                const specificationValue = {
                                                    value: element.innerText.trim()
                                                }
                                                specificationRow.specificationValues.push(specificationValue);
                                            });

                                            specification.specificationKeys.push(specificationRow); // Thêm thẻ li có class vào danh sách con của cha hiện tại
                                        }
                                    }
                                });
                                return JSON.stringify(gadget);
                            })()
                        ");
                        Gadget gadgetSpecificationDetail = JsonConvert.DeserializeObject<Gadget>(specificationDetail)!;
                        specialGadget.Specifications = gadgetSpecificationDetail.Specifications;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    listGadgets.Add(specialGadget);
                    Console.WriteLine("add success");

                    //Click nút close popup
                    moreDataToLoad = true;
                    while (moreDataToLoad && k < specialGadgetItemJsonResponses.Count - 1)
                    {
                        try
                        {
                            var clsButton = await page.WaitForSelectorAsync("a.cls", new WaitForSelectorOptions { Timeout = 2000 });

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
                                        var popup = await page.WaitForSelectorAsync("div#colorbox", new WaitForSelectorOptions { Timeout = 2000 });
                                        if (popup != null)
                                        {
                                            var isPopupVisible = await popup.IsVisibleAsync();
                                            if (isPopupVisible)
                                            {
                                                Console.WriteLine("popup visible");
                                            }
                                            else
                                            {
                                                Console.WriteLine("popup invisible");
                                                moreDataToLoad = false;
                                                await Task.Delay(2000);
                                            }
                                        }
                                    }
                                    catch (PuppeteerException ex)
                                    {
                                        Console.WriteLine("popup catch " + ex.Message);
                                        moreDataToLoad = true;
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
                    if (k < specialGadgetItemJsonResponses.Count - 1)
                    {
                        moreDataToLoad = true;
                        while (moreDataToLoad)
                        {
                            try
                            {
                                var gadgetTabBtn = await page.WaitForSelectorAsync($"section.wrapper.config.page1:not(.hide) div.tab a:nth-child({k + 2})", new WaitForSelectorOptions { Timeout = 2000 });

                                if (gadgetTabBtn != null)
                                {
                                    var isButtonVisible = await gadgetTabBtn.IsVisibleAsync();
                                    if (isButtonVisible)
                                    {
                                        Console.WriteLine($"gadgetTabBtn {k} visible");
                                        await gadgetTabBtn.ClickAsync();
                                        await Task.Delay(2000);
                                        try
                                        {

                                            bool isValid = await page.EvaluateExpressionAsync<bool>(
                                            $@"
                                            (function () {{
                                                const listLinkTabs = Array.from(document.querySelectorAll('section.wrapper.config.page1:not(.hide) div.tab a'));
                                                return listLinkTabs[{k + 1}].classList.contains('active');
                                            }})()
                                        ");
                                            if (isValid)
                                            {

                                                Console.WriteLine($"gadgetTab {k + 1} visible");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"gadgetTab {k + 1} invisible");
                                            }

                                            moreDataToLoad = !isValid;
                                            await Task.Delay(2000);
                                        }
                                        catch (PuppeteerException ex)
                                        {
                                            Console.WriteLine($"gadgetTab {k + 1} catch {ex.Message}");
                                            moreDataToLoad = true;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"gadgetTabBtn {k} invisible");
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
                                Console.WriteLine($"gadgetTabBtn {k} catch {ex.Message}");
                                moreDataToLoad = false;
                            }
                        }
                    }
                }
            }

            if (gadgetDetail.Name != "N/A")
            {
                gadgetDetail.Price = GadgetItemJsonResponses[i].Price != 0 ? GadgetItemJsonResponses[i].Price : null;
                gadgetDetail.ThumbnailUrl = GadgetItemJsonResponses[i].ThumbnailUrl;
                gadgetDetail.Url = page.Url;

                //Scrape Gadget Images
                string listGadgetImages = await page.EvaluateExpressionAsync<string>(
                    @"
                        (function () {
                            const listGadgetImages = [];
                            const imageElement = document.querySelector('div.box_left aside.thumb img');
                            if (imageElement) {
                                const gadgetImage = {
                                    imageUrl: imageElement.getAttribute('src')
                                }
                                listGadgetImages.push(gadgetImage);
                            }

                            const listOutstandingImages = Array.from(document.querySelectorAll('div.box_left div.feature-img div.owl-stage div.owl-item'));
                            for (let i = 0; i < listOutstandingImages.length; i++) {
                                const gadgetImage = {
                                    imageUrl: ''
                                }
                                const imgELement = listOutstandingImages[i].querySelector('div.item-img');
                                if (imgELement) {
                                    const imgSrc = imgELement.getAttribute('data-thumb');
                                    if (imgSrc) {
                                        gadgetImage.imageUrl = imgSrc;
                                        listGadgetImages.push(gadgetImage);
                                    }
                                }
                            }

                            const listImageElements = Array.from(document.querySelectorAll('div.gallery-img div.thubmail-slide.full div.owl-dots button.owl-dot img.theImg'));
                            for (let i = 0; i < listImageElements.length; i++) {
                                const gadgetImage = {
                                    imageUrl: ''
                                }
                                const imgSrc = listImageElements[i].getAttribute('src');
                                if (imgSrc) {
                                    gadgetImage.imageUrl = imgSrc;
                                    listGadgetImages.push(gadgetImage);
                                }
                            }

                            return JSON.stringify(listGadgetImages);
                        })()
                    ");
                List<GadgetImage> gadgetImages = JsonConvert.DeserializeObject<List<GadgetImage>>(listGadgetImages)!;
                gadgetDetail.GadgetImages = gadgetImages;

                //Scrape mô tả sản phẩm
                //Click bài viết đánh giá
                moreDataToLoad = true;
                while (moreDataToLoad)
                {
                    try
                    {
                        var descriptionBtn = await page.WaitForSelectorAsync("#tab-spec h2:not(.current)", new WaitForSelectorOptions { Timeout = 2000 });

                        if (descriptionBtn != null)
                        {
                            var isDescriptionBtnVisible = await descriptionBtn.IsVisibleAsync();
                            if (isDescriptionBtnVisible)
                            {
                                Console.WriteLine("descriptionBtn visible");
                                await descriptionBtn.ClickAsync();
                                await Task.Delay(2000);
                                try
                                {
                                    var descriptionContent = await page.WaitForSelectorAsync("div.description.tab-content.current", new WaitForSelectorOptions { Timeout = 2000 });
                                    if (descriptionContent != null)
                                    {
                                        var isPopupVisible = await descriptionContent.IsVisibleAsync();
                                        if (isPopupVisible)
                                        {
                                            Console.WriteLine("descriptionContent visible");
                                            moreDataToLoad = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("descriptionContent invisible");
                                        }
                                    }
                                }
                                catch (PuppeteerException ex)
                                {
                                    Console.WriteLine("descriptionContent catch " + ex.Message);
                                    moreDataToLoad = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("descriptionBtn invisible");
                                moreDataToLoad = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("descriptionBtn null");
                            moreDataToLoad = false;
                        }
                    }
                    catch (PuppeteerException ex)
                    {
                        Console.WriteLine("descriptionBtn catch " + ex.Message);
                        moreDataToLoad = false;
                    }
                }

                //Lấy data mô tả
                string listGadgetDescriptions = await page.EvaluateExpressionAsync<string>(
                @"
                    (function () {
                        const content = [];
                        const elements = document.querySelectorAll('div.text-detail > *'); // Chọn tất cả phần tử con của div.text-detail

                        elements.forEach((element, index) => {
                            if (element.tagName.toLowerCase() === 'h3') {
                                // Nếu là thẻ h3, đẩy vào mảng với loại là 'BoldText'
                                content.push({ type: 'BoldText', value: element.textContent.trim(), index: index });
                            } else if (element.tagName.toLowerCase() === 'p') {
                                const aElement = element.querySelector('a.preventdefault');
                                if (aElement) {
                                    content.push({ type: 'Image', value: aElement.getAttribute('href'), index: index });
                                } else {
                                    // Nếu là thẻ p, đẩy vào mảng với loại là 'NormalText'
                                    content.push({ type: 'NormalText', value: element.textContent.trim(), index: index });
                                }
                            }
                        });
                        return JSON.stringify(content);
                    })()
                ");
                List<GadgetDescription> gadgetDescriptions = JsonConvert.DeserializeObject<List<GadgetDescription>>(listGadgetDescriptions)!;
                gadgetDetail.GadgetDescriptions = gadgetDescriptions;

                listGadgets.Add(gadgetDetail);
                Console.WriteLine("add success");
            }

        }
        await browser.CloseAsync();

        return listGadgets;
    }
}
