namespace TgLang.CodeCrawler.Services.Contracts;

public interface ICodeCrawlerService
{
    Task CrawlAsync(string codeFolder, int sampleCount);
}