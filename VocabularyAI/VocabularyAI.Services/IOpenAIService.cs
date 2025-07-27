using VocabularyAI.Models;

namespace VocabularyAI.Services;

public interface IOpenAIService
{
    Task<ICollection<Word>> GenerateWordsAsync(Level level, string topic);
}
