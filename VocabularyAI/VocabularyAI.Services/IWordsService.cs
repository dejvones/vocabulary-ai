using VocabularyAI.Models;

namespace VocabularyAI.Services;

public interface IWordsService
{
    Task<ICollection<Word>> GenerateWordsAsync(Level level, string topic);
    bool IsCorrect(string czech, string english);
}
