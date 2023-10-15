using TgLang.CodeCrawler.Models;

namespace TgLang.CodeCrawler.Services.Contracts;

public interface ILanguageDefService
{
    /// <summary>
    /// Gets all defined languages
    /// </summary>
    /// <returns></returns>
    List<LanguageDef> GetLanguageDefs();
    
    /// <summary>
    /// Extracts the file extension and according language from a GitHub file url.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    (LanguageDef? language, string? extension) GetLanguageOfUrl(string url);
}