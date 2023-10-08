using TgLang.CodeCrawler.Models;

namespace TgLang.CodeCrawler.Services.Contracts;

public interface IGitHubService
{
    Task<List<string>> GetLanguageRepoUrlsAsync(LanguageDef languageDef);
}