using TgLang.CodeCrawler.Models;

namespace TgLang.CodeCrawler.Services.Contracts;

public interface IGitHubService
{
    Task<List<string>> GetLanguageRepoUrlsAsync(LanguageDef languageDef);
    Task<List<GitHubFile>> GetCodeFilesAsync(string folderUrl);
    Task<string> GetCodeFileContentAsync(long repositoryId, string sha);
    Task<List<GitHubFile>> SearchFilesAsync(LanguageDef language, int pageNo, int pageSize);
}