
using VocabularyAI.Models;

namespace VocabularyAI.Services;

public class WordsService(IOpenAIService openAIService) : IWordsService
{
    private ICollection<Word> _originalWords = [];
    private readonly IOpenAIService _openAIService = openAIService;

    public async Task<ICollection<Word>> GenerateWordsAsync(Level level, string topic)
    {
        var words = await _openAIService.GenerateWordsAsync(level, topic);
        //await Task.Delay(250);
        //ICollection<Word> words = [new Word("cz1", "en1"), new Word("cz2", "en2"), new Word("cz3", "en3")];
        _originalWords = words;
        return ShuffleWords(words);
    }

    public bool IsCorrect(string czech, string english)
    {
        if (_originalWords.Count == 0)
            return false;
        return _originalWords.Any(w => w.Czech.Equals(czech, StringComparison.OrdinalIgnoreCase) &&
                                       w.English.Equals(english, StringComparison.OrdinalIgnoreCase));
    }

    private static List<Word> ShuffleWords(ICollection<Word> words)
    {
        var czechList = words.Select(w => w.Czech).ToList();
        var englishList = words.Select(w => w.English).ToList();

        var rng = new Random();
        czechList = [.. czechList.OrderBy(_ => rng.Next())];
        englishList = [.. englishList.OrderBy(_ => rng.Next())];

        var count = Math.Min(czechList.Count, englishList.Count);
        var result = new List<Word>(count);

        for (int i = 0; i < count; i++)
        {
            result.Add(new Word(czechList[i], englishList[i]));
        }

        return result;
    }
}

public enum Level
{
    A1,
    A2,
    B1,
    B2,
    C1,
    C2
}
