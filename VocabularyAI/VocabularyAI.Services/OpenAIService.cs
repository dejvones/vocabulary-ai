using OpenAI.Chat;
using System.Text.RegularExpressions;
using VocabularyAI.Models;

namespace VocabularyAI.Services;

public partial class OpenAIService : IOpenAIService
{
    private const string ModelName = "gpt-4.1-nano";
    private const string SystemMessage = "Jsi AI učitel angličtiny, který pomáhá s učením slovní zásoby. Generuj dvojice českých a anglických slov dle zadání ve formátu: 1. česky - anglicky 2. česky - anglicky.";
    private const string UserMessage = "Vygeneruj 10 dvojic, které jsou významově podobné a ne úplně jednoduché na úrovni";

    private readonly ChatClient _client;

    public OpenAIService()
    {
        _client = new ChatClient(ModelName, Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }

    public async Task<ICollection<Word>> GenerateWordsAsync(Level level, string topic)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(SystemMessage),
            new UserChatMessage($"{UserMessage} {GetLevelString(level)} na téma {topic}")
        };

        var result = await _client.CompleteChatAsync(messages);
        var response = result.Value.Content.FirstOrDefault()?.Text;

        return string.IsNullOrEmpty(response) ? [] : ParseResponse(response);
    }

    private static string GetLevelString(Level level)
    {
        return level switch
        {
            Level.A1 => "A1",
            Level.A2 => "A2",
            Level.B1 => "B1",
            Level.B2 => "B2",
            Level.C1 => "C1",
            Level.C2 => "C2",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    private static List<Word> ParseResponse(string response)
    {
        var words = new List<Word>();
        var regex = WordPairRegex();

        foreach (Match match in regex.Matches(response))
        {
            var czech = match.Groups["cz"].Value.Trim('\r', '\n', '\t');
            var english = match.Groups["en"].Value.Trim('\r', '\n', '\t');

            if (!string.IsNullOrEmpty(czech) && !string.IsNullOrEmpty(english))
            {
                words.Add(new Word(czech, english));
            }
        }

        return words;
    }

    // Matches: 1. cz - en 2. cz - en ... on a single line or multiple lines
    [GeneratedRegex(@"\d+\.\s*(?<cz>.+?)\s*[-–]\s*(?<en>.*?)(?=\s*\d+\.|$)", RegexOptions.Singleline)]
    private static partial Regex WordPairRegex();
}
