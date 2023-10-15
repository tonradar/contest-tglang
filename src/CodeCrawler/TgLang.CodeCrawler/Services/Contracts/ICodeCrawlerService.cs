namespace TgLang.CodeCrawler.Services.Contracts;

public interface ICodeCrawlerService
{
    Task CrawlUsingReposAsync(string codeFolder, int sampleCount);
    Task CrawlUsingSearchAsync(string codeFolder, int sampleCount);
}