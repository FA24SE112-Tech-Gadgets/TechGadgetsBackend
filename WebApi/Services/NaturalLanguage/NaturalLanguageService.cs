using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Text.Json;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Services.Embedding;
using WebApi.Services.NaturalLanguage.Models;

namespace WebApi.Services.AI;

public class NaturalLanguageService(IOptions<OpenAIClientSettings> options, AppDbContext context, EmbeddingService embeddingService)
{
    private readonly OpenAIClientSettings _settings = options.Value;

    public async Task<NaturalLanguageRequest?> GetRequestByUserInput(string input)
    {
        List<string> purposes =
        [
            "Học tập", "Làm việc", "Văn phòng", "Gaming", "Giải trí",
            "Vận động", "Ngoài trời", "Thể thao", "Thiết kế đồ họa",
            "Designer", "IT", "Bơi lội"
        ];

        List<string> brands = await context.Brands.Select(b => b.Name).ToListAsync();

        List<string> categories = await context.Categories.Select(c => c.Name).ToListAsync();

        List<string> highResolutionKeywords = [
            "Full HD", "QQVGA", "QVGA", "2K", "1.5K", "Retina", "4k"
            ];

        List<string> operatingSystems = ["Windows", "Android", "Linux", "MacOS", "ChromeOS", "iOS", "Táo"];

        List<string> storageCapacitiesPhone = ["32GB", "64GB", "128GB", "256GB", "512GB", "1TB", "2TB"];

        List<string> storageCapacitiesLaptop = ["256GB", "512GB", "1TB", "2TB"];

        List<string> RAMs = ["4GB", "8GB", "12GB", "16GB", "32GB", "64GB"];

        List<string> features = ["Quay góc rộng", "Chống nước", "Chống sốc", "Khóa vân tay", "Nhận diện khuôn mặt",
                                 "Kháng nước", "Công nghệ NFC", "Ban đêm", "Camera chống rung",
                                 "Hỗ trợ 5G", "Hỗ trợ 4G", "Sạc không dây", "Dành cho chơi game, lướt web, xem phim"];

        string myPrompt = $@"
        I have data in postgres of gadgets (phone, laptop, speaker, earphone, headphone,...) that user can search.

        purposes are: {string.Join(", ", purposes)}
        If user query not mention, give me empty array


        brands are: {string.Join(", ", brands)}
        If user query not mention, give me empty array


        categories are: {string.Join(", ", categories)}
        If user query not mention, give me empty array


        isFastCharge can be true or false
        If user does not mention, give me false


        isGoodBatteryLife can be true or false
        If user does not mention, give me false


        UsageTime is in hours
        minUsageTime must be greater than or equal to 0
        If user does not mention, give me min value, which is 0
        
        maxUsageTime must be less than or equal to 48
        If user does not mention, give me max value, which is 48
       
        
        isWideScreen can be true or false
        If user does not mention, give me false


        isFoldable can be true or false
        If user does not mention, give me false        


        Inch 
        minInch must be greater than or equal to 0
        If user does not mention, give me min value, which is 0
        
        maxInch must be less than or equal to 30
        If user does not mention, give me max value, which is 30


        isHighResolution can be true or false
        you can use this keywork array as a addition reference that results in isHighResolution is true: {string.Join(", ", highResolutionKeywords)}
        If user does not mention, give me false 


        operatingSystems are: {string.Join(", ", operatingSystems)}
        If user query not mention, give me empty array       


        storageCapacitiesPhone are: {string.Join(", ", storageCapacitiesPhone)}
        If user query not mention, give me empty array


        storageCapacitiesLaptop are: {string.Join(", ", storageCapacitiesLaptop)}
        If user query not mention, give me empty array


        RAMs are: {string.Join(", ", RAMs)}
        If user query not mention, give me empty array


        features are: {string.Join(", ", features)}
        If user query not mention, give me empty array


        User's query: {input}
        ";

        List<ChatMessage> messages =
        [
            new UserChatMessage(myPrompt),
        ];

        ChatClient client = new(_settings.Model, _settings.Key);

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
                        "isFastCharge": {
                            "type": "boolean"
                        },
                        "isGoodBatteryLife": {
                            "type": "boolean"
                        },
                        "minUsageTime": {
                            "type": "number"
                        },
                        "maxUsageTime": {
                            "type": "number"
                        },
                        "isWideScreen": {
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
                        }
                    },
                    "required": ["purposes","brands","categories","isFastCharge","isGoodBatteryLife","minUsageTime","maxUsageTime","isWideScreen","isFoldable",
                                 "minInch","maxInch","isHighResolution","operatingSystems","storageCapacitiesPhone","storageCapacitiesLaptop","rams","features"],
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
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
        }

        filter!.InputVector = await embeddingService.GetEmbedding(input);

        return filter;
    }
}
