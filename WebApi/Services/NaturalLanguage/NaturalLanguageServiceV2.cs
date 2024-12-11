using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Text.Json;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Data.Entities;
using WebApi.Services.Cryption;
using WebApi.Services.NaturalLanguage.Models;

namespace WebApi.Services.NaturalLanguage;

public class NaturalLanguageServiceV2(IOptions<OpenAIClientSettings> options, AppDbContext context, AesEncryptionService aesEncryptionService)
{
    private readonly OpenAIClientSettings _settings = options.Value;

    public async Task<NaturalLanguageRequestV2?> GetRequestByUserInput(string input)
    {
        input = input.Length > 512 ? input[0..512] : input;

        List<string> brands = await context.Brands.Select(b => b.Name).ToListAsync();

        List<string> categories = await context.Categories.Select(c => c.Name).ToListAsync();

        List<string> keywords = await context.NaturalLanguageKeywords
                                    .Where(k => k.NaturalLanguageKeywordGroup.Status == NaturalLanguageKeywordGroupStatus.Active
                                    && k.Status == NaturalLanguageKeywordStatus.Active)
                                    .OrderBy(k => k.NaturalLanguageKeywordGroup.Id)
                                    .Select(k => k.Keyword)
                                    .ToListAsync();

        var groups = await context.NaturalLanguageKeywordGroups
                                .Include(g => g.NaturalLanguageKeywords.Where(k => k.Status == NaturalLanguageKeywordStatus.Active))
                                .Where(g => g.Status == NaturalLanguageKeywordGroupStatus.Active)
                                .ToListAsync();

        string keywordGroups = "";
        foreach (var group in groups)
        {
            string tmp = $"Group Name: {group.Name}. Group Keywords: {string.Join(", ", group.NaturalLanguageKeywords.Select(k => k.Keyword))}";
            keywordGroups += tmp;
            keywordGroups += '\n';
        }

        List<string> storageCapacitiesPhone = ["32GB", "64GB", "128GB", "256GB", "512GB", "1TB", "2TB"];

        List<string> storageCapacitiesLaptop = ["256GB", "512GB", "1TB", "2TB"];

        List<string> RAMs = ["4GB", "8GB", "12GB", "16GB", "32GB", "64GB", "128GB"];

        List<string> searchingSellerKeywords = ["Cửa hàng", "Nơi bán", "Người bán", "Shop"];

        List<string> bestGadgetKeywords = ["Sản phẩm được nhiều người quan tâm", "Nhiều người mua nhất", "Bán chạy", "Sản phẩm bán chạy", "Nổi bật"];

        List<string> highRatingKeywords = ["Đánh giá cao"];

        List<string> positiveReviewKeywords = ["Tích cực", "Đánh giá tích cực"];

        List<string> discountKeywords = ["Giảm giá", "Khuyến mãi"];

        List<string> bestSellerKeywords = ["nổi bật", "bán chạy"];

        List<string> availableKeywords = ["Còn hàng"];

        List<string> endOfYearKeywords = ["Cuối năm", "Cuối"];

        List<string> startOfYearKeywords = ["Đầu năm", "Đầu"];

        string myPrompt = $@"
        I have data in postgres of gadgets (phone, laptop, speaker, earphone, headphone,...) that user can search.
        Now is December, 2024

        brands are: {string.Join(", ", brands)}
        please use the keywork array above as a reference
        If user query not mention anything about brands, give me empty array


        categories are: {string.Join(", ", categories)}
        please use the keywork array above as a reference
        If user query not mention anything about categories, give me empty array


        Price is the price of gadgets
        minPrice must be greater than or equal to 0
        
        maxPrice must be less than or equal to 150000000


        operatingSystems is operating systems of gadgets
        If user query not mention, give me empty array       


        storageCapacitiesPhone are: {string.Join(", ", storageCapacitiesPhone)}
        If user query not mention, give me empty array


        storageCapacitiesLaptop are: {string.Join(", ", storageCapacitiesLaptop)}
        If user query not mention, give me empty array


        RAMs are: {string.Join(", ", RAMs)}
        If user query not mention, give me empty array


        locations are the place gadgets being sold
        If user query not mention, give me empty array


        origins are the place gadgets being made/manufactured
        If user query not mention, give me empty array

        
        releaseDate can be year which the string format is YYYY or can be month/year which the string format is MM/YYYY
        only one of the above format, nothing else
        if user mention end of the year, using some words like {string.Join(", ", endOfYearKeywords)} then take these months: 9, 10, 11, 12.
        if user mention start of the year, using some words like {string.Join(", ", startOfYearKeywords)} then take these months: 1, 2, 3, 4.
        ONLY add result if user DID mention about release date. If user does not mention, give me empty array
        

        colors are the colors of gadgets
        If user query not mention, give me empty array          
       

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
           

        isDiscounted can be true or false
        you can use this keywork array as a addition reference that results in isDiscounted is true: {string.Join(", ", discountKeywords)}
        If user does not mention, give me false        
        When isDiscounted is true, give me minDiscount and maxDiscount, in range 0..100        


        isBestSeller can be true or false
        you can use this keywork array as a addition reference that results in isBestSeller is true: {string.Join(", ", bestSellerKeywords)}
        If user does not mention, give me false  


        isAvailable can be true or false
        you can use this keywork array as a addition reference that results in isAvailable is true: {string.Join(", ", availableKeywords)}
        If user does not mention, give me false
        
    
        sellerName is the name of the shop/seller in the query
        usually user will mention this after mention some keywords: {string.Join(", ", searchingSellerKeywords)}
        If user does not mention, give me empty string


        keywords are: 
        {string.Join(", ", keywords)}

        Please only add to the result array if it satifies both condition, don't just add random keywords: 
        - appears both in the array above and in the user's query
        - and match 100% the WHOLE keyword phrase, not just some part

        User's query: {input}
        ";

        string z = $"""
            keywords are: 
            {string.Join("\n", keywords)}
         
            Please only add to the result array if it satifies both condition, don't just add random keywords: 
            - appears both in the array above and in the user's query
            - and match 100% the WHOLE phrase, if keyword match 100% but not WHOLE phrase then don't add 

            keyword groups are: 
            {keywordGroups}
            If any exact 100% match of the keywords from the above list is mentioned in the user's query, only then add the corresponding keyword group to the result array.
         """;

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
                        "isDiscounted": {
                            "type": "boolean"
                        },
                        "minDiscount": {
                            "type": "number"
                        },
                        "maxDiscount": {
                            "type": "number"
                        },
                        "isBestSeller": {
                            "type": "boolean"
                        },
                        "isAvailable": {
                            "type": "boolean"
                        },
                        "sellerName": {
                            "type": "string"
                        },
                        "keywords": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        }
                    },
                    "required": ["brands","categories","minPrice","maxPrice",
                                 "operatingSystems","storageCapacitiesPhone","storageCapacitiesLaptop","rams","locations","origins","releaseDate","colors","isSearchingSeller",
                                 "isBestGadget","isHighRating","isPositiveReview","isDiscounted","minDiscount","maxDiscount",
                                 "isBestSeller","isAvailable","sellerName","keywords"],
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

        NaturalLanguageRequestV2? filter = null;
        try
        {
            filter = JsonSerializer.Deserialize<NaturalLanguageRequestV2>(completion.Content[0].Text, new JsonSerializerOptions
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
}
