using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Text.Json;
using WebApi.Common.Settings;
using WebApi.Data;
using WebApi.Services.Cryption;
using WebApi.Services.NaturalLanguage.Models;

namespace WebApi.Services.NaturalLanguage;

public class NaturalLanguageServiceV2(IOptions<OpenAIClientSettings> options, AppDbContext context, AesEncryptionService aesEncryptionService)
{
    private readonly OpenAIClientSettings _settings = options.Value;

    public async Task<NaturalLanguageRequestV2?> GetRequestByUserInput(string input)
    {
        input = input.Length > 512 ? input[0..512] : input;

        List<string> keywords =
        [
            "Học tập", "Làm việc", "Văn phòng", "Gaming", "Giải trí",
            "Vận động", "Ngoài trời", "Thể thao", "Thiết kế đồ họa",
            "Designer", "IT", "Bơi lội", "Pin trâu", "Pin khỏe", "Thời gian sử dụng cao", "Thời lượng pin trâu", "Dung lượng pin trâu",
            "Giá rẻ", "Giá tốt", "Giá sinh viên", "Tầm trung", "Cao cấp", "Hiện đại"
        ];

        string myPrompt = $@"
        I have data in postgres of gadgets (phone, laptop, speaker, earphone, headphone,...) that user can search.
        Now is November, 2024

        keywords are: {string.Join(", ", keywords)}
        please use the keywork array above as a reference to give me array of keywords user mention
        If user query not mention anything about purposes, give me empty array



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
                        "keywords": {
                            "type": "array",
                            "items": {
                                "type": "string"
                            }
                        }
                    },
                    "required": ["keywords"],
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
