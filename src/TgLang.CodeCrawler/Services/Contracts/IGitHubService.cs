using TgLang.CodeCrawler.Models;

namespace TgLang.CodeCrawler.Services.Contracts;

public interface IGitHubService
{
    Task<List<string>> GetLanguageRepoUrlsAsync(LanguageDef languageDef);
    Task<List<GitHubFile>> GetFilesByUrlAsync(string folderUrl);
    Task<string> GetCodeFileContentAsync(long repositoryId, string sha);
    Task<List<GitHubFile>> GetFilesBySearchAsync(LanguageDef language, int pageNo, int pageSize);
    Task<List<GitHubFile>> GetFilesByTagAsync(LanguageDef language, int sampleCount);
}