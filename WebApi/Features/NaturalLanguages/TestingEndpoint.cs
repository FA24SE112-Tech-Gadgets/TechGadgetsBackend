using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Services.Embedding;

namespace WebApi.Features.NaturalLanguages;

[ApiController]
public class TestingEndpoint : ControllerBase
{
    [Tags("Natural Language")]
    [HttpPost("natural-languages/test")]
    public async Task<IActionResult> Handler(AppDbContext context, EmbeddingService embeddingService)
    {
        List<string> storageCapacitiesPhone = ["32GB", "64GB", "128GB", "256GB", "512GB", "1TB", "2TB"];
        List<(int number, string)> list = storageCapacitiesPhone.Select(ExtractNumberAndString).ToList();


        return Ok();
    }

    private static (int number, string text) ExtractNumberAndString(string input)
    {
        // Find the index where the numeric part ends
        int index = 0;
        while (index < input.Length && char.IsDigit(input[index]))
        {
            index++;
        }

        // Extract the number and the string
        string numberPart = input.Substring(0, index);
        string stringPart = input.Substring(index);

        // Convert the number part to an integer
        int number = int.Parse(numberPart);

        return (number, stringPart);
    }

    public static void Main()
    {
        string input = "122310ababcbc";
        var result = ExtractNumberAndString(input);
        Console.WriteLine($"Number: {result.number}, String: {result.text}");

        input = "0904904dlkejfmwp";
        result = ExtractNumberAndString(input);
        Console.WriteLine($"Number: {result.number}, String: {result.text}");
    }

}
