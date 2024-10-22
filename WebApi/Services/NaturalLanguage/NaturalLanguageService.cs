using Microsoft.Extensions.Options;
using WebApi.Common.Settings;
using WebApi.Services.NaturalLanguage.Models;

namespace WebApi.Services.AI;

public class NaturalLanguageService(IOptions<OpenAIClientSettings> options)
{
    private readonly OpenAIClientSettings _settings = options.Value;

    public async Task<NaturalLanguageRequest> GetRequestByUserInput(string input)
    {
        return new NaturalLanguageRequest();
    }
}
