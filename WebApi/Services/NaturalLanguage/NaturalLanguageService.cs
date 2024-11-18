using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Text.Json;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Services.Cryption;
using WebApi.Services.NaturalLanguage.Models;

namespace WebApi.Services.AI;

public class NaturalLanguageService(IOptions<OpenAIClientSettings> options, AppDbContext context, AesEncryptionService aesEncryptionService)
{
    private readonly OpenAIClientSettings _settings = options.Value;

    public async Task<NaturalLanguageRequest?> GetRequestByUserInput(string input)
    {
        input = input.Length > 512 ? input[0..512] : input;

        List<string> purposes =
        [
            "Học tập", "Làm việc", "Văn phòng", "Gaming", "Giải trí",
            "Vận động", "Ngoài trời", "Thể thao", "Thiết kế đồ họa",
            "Designer", "IT", "Bơi lội"
        ];

        List<string> brands = await context.Brands.Select(b => b.Name).ToListAsync();

        List<string> categories = await context.Categories.Select(c => c.Name).ToListAsync();

        List<string> fastChargeKeywords = ["Sạc nhanh", "Sạc pin nhanh"];

        List<string> goodBatteryLifeKeywords = ["Pin trâu", "Pin khỏe", "Thời gian sử dụng cao", "Thời lượng pin trâu", "Dung lượng pin trâu", "Chiến game vô tư"];

        List<string> highResolutionKeywords = [
            "Full HD", "QQVGA", "QVGA", "2K", "1.5K", "Retina", "4k", "Độ phân giải cao", "Độ phân giải sắc nét"
            ];

        List<string> operatingSystems = ["Windows", "Android", "Linux", "MacOS", "ChromeOS", "iOS", "Táo"];

        List<string> storageCapacitiesPhone = ["32GB", "64GB", "128GB", "256GB", "512GB", "1TB", "2TB"];

        List<string> storageCapacitiesLaptop = ["256GB", "512GB", "1TB", "2TB"];

        List<string> RAMs = ["4GB", "8GB", "12GB", "16GB", "32GB", "64GB"];

        List<string> features = ["Quay góc rộng", "Chống nước", "Chống sốc", "Khóa vân tay", "Nhận diện khuôn mặt",
                                 "Kháng nước", "Công nghệ NFC", "Ban đêm", "Camera chống rung", "Bảo mật vân tay",
                                 "Hỗ trợ 5G", "Hỗ trợ 4G", "Sạc không dây", "Dành cho chơi game, lướt web, xem phim"];

        List<string> conditions = ["Mới", "Móp, méo", "Đã qua sử dụng", "Còn phiếu bảo hành", "Nguyên tem nguyên seal", "Vỡ màn hình",
                                        "Lỗi phần cứng", "Pin 90%, 85%", "Hỏng màn hình", "Trầy xước", "Tình trạng còn zin"];

        List<string> segmentations = ["Giá rẻ", "Giá tốt", "Giá sinh viên", "Tầm trung", "Cao cấp", "Hiện đại"];

        List<string> locations = ["Hà Nội", "Hồ Chí Minh", "Cần Thơ", "Đà Nẵng", "Quy Nhơn", "Hải Phòng"];

        List<string> origins = ["Việt Nam", "Trung Quốc", "Mỹ", "Nhật Bản", "Hàn Quốc", "Đài Loan", "Mỹ", "Nhật Bản", "Ấn Độ", "Đức", "Brazil", "Mexico"];

        List<string> colors = ["Xám", "Xanh Đen", "Bạc", "Vàng", "Cam", "Đỏ", "Tím", "Nâu",
        "Đen", "Xanh lá", "Xanh dương", "Titan tự nhiên", "Titan Sa Mạc", "Titan Trắng", "Titan Đen", "Xanh Lưu Ly", "Hồng", "Xanh Mòng Két", "Trắng", "Trắng ngọc trai"];

        List<string> AIKeywords = ["AI", "trí tuệ nhân tạo"];

        List<string> searchingSellerKeywords = ["Cửa hàng", "Nơi bán", "Nhà phân phối", "Thương hiệu", "Người bán", "Shop", "Nhà cung cấp", "Nơi cung cấp", "Hãng cung cấp", "Người cung cấp"];

        List<string> bestGadgetKeywords = ["Sản phẩm được nhiều người quan tâm", "Nhiều người mua nhất", "Bán chạy", "Sản phẩm bán chạy", "Nổi bật"];

        List<string> highRatingKeywords = ["Đánh giá cao"];

        List<string> positiveReviewKeywords = ["Tích cực", "Đánh giá tích cực"];

        List<string> energySavingKeywords = ["Tiết kiệm điện", "Xài điện ít", "Tiêu thụ điện thấp"];

        List<string> discountKeywords = ["Giảm giá", "Khuyến mãi"];

        List<string> bestSellerKeywords = ["Cửa hàng nổi bật", "bán chạy", "Nhiều người quan tâm"];

        List<string> availableKeywords = ["Còn hàng"];

        List<string> categoryTypeKeywords = ["Tai nghe Bluetooth", "Tai nghe có dây", "Tai nghe chụp tai", "Tai nghe gaming",
                                            "Loa Bluetooth","Loa kéo","Loa karaoke","Loa điện","Loa vi tính","Loa thanh"];

        string myPrompt = $@"
        I have data in postgres of gadgets (phone, laptop, speaker, earphone, headphone,...) that user can search.
        Now is November, 2024

        purposes are: {string.Join(", ", purposes)}
        If user query not mention, give me empty array


        brands are: {string.Join(", ", brands)}
        If user query not mention, give me empty array


        categories are: {string.Join(", ", categories)}
        If user query not mention, give me empty array


        Price
        minPrice must be greater than or equal to 0
        If user does not mention, give me min value, which is 0
        
        maxPrice must be less than or equal to 150000000
        If user does not mention, give me max value, which is 150000000


        isFastCharge can be true or false
        please use this keywork array as a addition reference that results in isFastCharge is true: {string.Join(", ", fastChargeKeywords)}
        If user does not mention, give me false 


        isGoodBatteryLife can be true or false
        please use this keywork array as a addition reference that results in isGoodBatteryLife is true: {string.Join(", ", goodBatteryLifeKeywords)}
        If user does not mention, give me false 


        UsageTime is in hours
        minUsageTime must be greater than or equal to 0
        If user does not mention, give me min value, which is 0
        
        maxUsageTime must be less than or equal to 48
        If user does not mention, give me max value, which is 48
       
        
        isWideScreen can be true or false
        If user does not mention, give me false

        
        isSmallScreen can be true or false
        If user does not mention, give me false        


        isFoldable can be true or false
        If user does not mention, give me false        


        Inch 
        minInch must be greater than or equal to 0
        If user does not mention, give me min value, which is 0
        
        maxInch must be less than or equal to 30
        If user does not mention, give me max value, which is 30


        isHighResolution can be true or false
        If user does not mention, give me false 


        operatingSystems 
        If user query not mention, give me empty array       


        storageCapacitiesPhone are: {string.Join(", ", storageCapacitiesPhone)}
        If user query not mention, give me empty array


        storageCapacitiesLaptop are: {string.Join(", ", storageCapacitiesLaptop)}
        If user query not mention, give me empty array


        RAMs are: {string.Join(", ", RAMs)}
        If user query not mention, give me empty array


        features are: {string.Join(", ", features)}
        If user query not mention, give me empty array


        conditions are: {string.Join(", ", conditions)}
        If user query not mention, give me empty array


        segmentations are: {string.Join(", ", segmentations)}
        Only give me items in the array above, if user query not mention anything in the array above, give me empty array


        locations are: {string.Join(", ", locations)}
        If user query not mention, give me empty array

        
        origins are: {string.Join(", ", origins)}
        If user query not mention, give me empty array

        
        releaseDate can be year which the string format is YYYY or can be month/year which the string format is MM/YYYY, EITHER of these only
        if user does not mention, give me empty array
        

        colors are: {string.Join(", ", colors)}
        If user query not mention, give me empty array            


        isAI can be true or false
        please use this keywork array as a reference that results in isAI is true: {string.Join(", ", AIKeywords)}
        If user does not mention, give me false  
        

        isSearchingSeller can be true or false, this field is true when user want to find seller not gadget
        please use this keywork array as a reference that results in isSearchingSeller is true: {string.Join(", ", searchingSellerKeywords)}
        If user does not mention, give me false  


        isBestGadget can be true or false, this field is true when user want to find best gadgets
        you can use this keywork array as a addition reference that results in isBestGadget is true: {string.Join(", ", bestGadgetKeywords)}
        If user does not mention, give me false
        

        isHighRating can be true or false, this field is true when user want to find gadgets with high ratings
        you can use this keywork array as a addition reference that results in isBestGadget is true: {string.Join(", ", highRatingKeywords)}
        If user does not mention, give me false


        isPositiveReview can be true or false, this field is true when user want to find gadgets with positive reviews
        you can use this keywork array as a addition reference that results in isBestGadget is true: {string.Join(", ", positiveReviewKeywords)}
        If user does not mention, give me false
             

        isEnergySaving can be true or false, this field is true when user want to find gadgets that are energy saving
        you can use this keywork array as a addition reference that results in isBestGadget is true: {string.Join(", ", energySavingKeywords)}
        If user does not mention, give me false


        isDiscounted can be true or false
        you can use this keywork array as a addition reference that results in isDiscounted is true: {string.Join(", ", discountKeywords)}
        If user does not mention, give me false        


        isBestSeller can be true or false
        you can use this keywork array as a addition reference that results in isBestSeller is true: {string.Join(", ", bestSellerKeywords)}
        If user does not mention, give me false  


        isAvailable can be true or false
        you can use this keywork array as a addition reference that results in isAvailable is true: {string.Join(", ", availableKeywords)}
        If user does not mention, give me false      


        categoryType are: {string.Join(", ", categoryTypeKeywords)}
        If user query not mention, give me empty array     

        User's query: {input}
        ";

        List<ChatMessage> messages =
        [
            new UserChatMessage(myPrompt),
        ];

        var apiKey = aesEncryptionService.Decrypt(_settings.EncryptedKey);
        ChatClient client = new(_settings.StructuredOutputModel, apiKey);

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName: "gadget_filter",
            jsonSchema: BinaryData.FromBytes("""
                {
                    "type": "object",
                    "properties": {
                        "purposes": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "brands": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "categories": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "minPrice": {
                            "type": "number"
                        },
                        "maxPrice": {
                            "type": "number"
                        },
                        "isFastCharge": {
                            "type": "boolean"
                        },
                        "isGoodBatteryLife": {
                            "type": "boolean"
                        },
                        "isWideScreen": {
                            "type": "boolean"
                        },
                        "isSmallScreen": {
                            "type": "boolean"
                        },
                        "isFoldable": {
                            "type": "boolean"
                        },
                        "minInch": {
                            "type": "number"
                        },
                        "maxInch": {
                            "type": "number"
                        },
                        "isHighResolution": {
                            "type": "boolean"
                        },
                        "operatingSystems": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "storageCapacitiesPhone": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "storageCapacitiesLaptop": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "rams": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "features": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "conditions": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "segmentations": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "locations": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "origins": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "releaseDate": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "colors": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                        "isAi": {
                            "type": "boolean"
                        },
                        "isSearchingSeller": {
                            "type": "boolean"
                        },
                        "isBestGadget": {
                            "type": "boolean"
                        },
                        "isHighRating": {
                            "type": "boolean"
                        },
                        "isPositiveReview": {
                            "type": "boolean"
                        },
                        "isEnergySaving": {
                            "type": "boolean"
                        },
                        "isDiscounted": {
                            "type": "boolean"
                        },
                        "isBestSeller": {
                            "type": "boolean"
                        },
                        "isAvailable": {
                            "type": "boolean"
                        },
                        "categoryTypes": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        },
                    },
                    "required": ["purposes","brands","categories","minPrice","maxPrice","isFastCharge","isGoodBatteryLife","isWideScreen","isSmallScreen",
                                 "isFoldable","minInch","maxInch","isHighResolution","operatingSystems","storageCapacitiesPhone","storageCapacitiesLaptop","rams","features",
                                 "conditions","segmentations","locations","origins","releaseDate","colors","isAi","isSearchingSeller",
                                 "isBestGadget","isHighRating","isPositiveReview","isEnergySaving","isDiscounted","isBestSeller","isAvailable","categoryTypes"],
                    "additionalProperties": false
                }
                """u8.ToArray()),
            jsonSchemaIsStrict: true)
        };

        ChatCompletion? completion = null;
        try
        {
            completion = await client.CompleteChatAsync(messages, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chat complete error: {ex.Message}");
            return null;
        }

        using JsonDocument structuredJson = JsonDocument.Parse(completion!.Content[0].Text);
        JsonElement root = structuredJson.RootElement;

        NaturalLanguageRequest? filter = null;
        try
        {
            filter = JsonSerializer.Deserialize<NaturalLanguageRequest>(completion.Content[0].Text, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        catch (Exception jsonEx)
        {
            Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
        }

        return filter;
    }

    public async Task<bool?> IsPositiveContent(string content)
    {
        content = content.Length > 512 ? content[0..512] : content;

        string myPrompt = $@"
        I need you to check if this content is positive or not, this is content of product review:

        Content: {content}
        ";

        List<ChatMessage> messages =
        [
            new UserChatMessage(myPrompt),
        ];

        var apiKey = aesEncryptionService.Decrypt(_settings.EncryptedKey);
        ChatClient client = new(_settings.StructuredOutputModel, apiKey);

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName: "is_positive",
            jsonSchema: BinaryData.FromBytes("""
                {
                    "type": "object",
                    "properties": {

                        "isPositive": {
                            "type": "boolean"
                        }
                    },
                    "required": ["isPositive"],
                    "additionalProperties": false
                }
                """u8.ToArray()),
            jsonSchemaIsStrict: true)
        };

        ChatCompletion? completion = null;
        try
        {
            completion = await client.CompleteChatAsync(messages, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chat complete error: {ex.Message}");
            return null;
        }

        using JsonDocument structuredJson = JsonDocument.Parse(completion!.Content[0].Text);
        JsonElement root = structuredJson.RootElement;

        IsPositiveRequest? res = null;
        try
        {
            res = JsonSerializer.Deserialize<IsPositiveRequest>(completion.Content[0].Text, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        catch (Exception jsonEx)
        {
            Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
        }

        return res!.IsPositive;
    }
}
