using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PuppeteerSharp;
using WebApi.Data.Entities;
using WebApi.Services.Background.GadgetScrapeData.Models;

namespace WebApi.Services.ScrapeData;

public class ScrapeTGDDDataService
{
    public async Task<string> ScrapeGadgetByBrand(string url, string gadgetCate, string phoneBrand)
    {
        string defaultUrl = CutUrl(url);
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

        while (moreDataToLoad)
        {
            try
            {
                var initialCount = await page.EvaluateFunctionAsync<int>(@"
                    () => {
                        const ul = Array.from(document.querySelectorAll('ul.listproduct li.item')).length;
                        return ul;
                    }
                ");
                Console.WriteLine(initialCount);
                var button = await page.WaitForSelectorAsync("div.view-more a", new WaitForSelectorOptions { Timeout = 2000 });

                if (button != null)
                {
                    Console.WriteLine("button co");
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
                moreDataToLoad = false;
            }
        }
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

            Gadget gadgetDetail = new Gadget()!;
            try
            {
                string jsonGadgetDetail = "";
                if (gadgetCate == "Điện thoại" || gadgetCate == "Laptop")
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
                }
                else
                {
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
                }
                gadgetDetail = JsonConvert.DeserializeObject<Gadget>(jsonGadgetDetail)!;
                Console.WriteLine("ktra: " + gadgetDetail.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //TH các điện thoại có kiểu style đặc biệt cần phải vào lấy rõ hơn
            if (gadgetDetail.Name == "N/A" && phoneBrand != "iPhone")
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
                        specialGadget.Price = ConvertPriceToInt(priceWithSpecificationDetail);

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
                                    const urlMatch = backgroundUrl.match(/url\(\s*['\""]?(.*?)['\""]?\s*\)/);
                                    return urlMatch ? urlMatch[1] : 'khong match url';
                                }
                            }
                            return 'khong thay url';
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
                                        name: li.querySelector(""label"").textContent.trim(),   // thẻ li không có class
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
                                                name: element.innerText.trim()
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
                    specialGadget.Name = "Điện thoại Samsung " + specialGadgetItemJsonResponses[k];
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
                        specialGadget.Price = ConvertPriceToInt(priceWithSpecificationDetail);

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
                                    const urlMatch = backgroundUrl.match(/url\(\s*['\""]?(.*?)['\""]?\s*\)/);
                                    return urlMatch ? urlMatch[1] : 'khong match url';
                                }
                            }
                            return 'khong thay url';
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
                                        name: li.querySelector(""label"").textContent.trim(),   // thẻ li không có class
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
                                                name: element.innerText.trim()
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

        return JsonConvert.SerializeObject(listGadgets);
    }

    private static string CutUrl(string url)
    {
        var uri = new Uri(url);
        return $"{uri.Scheme}://{uri.Host}";
    }

    private static int ConvertPriceToInt(string priceString)
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
