namespace TgLang.CodeCrawler.Services.Contracts;

public interface ICodeCrawlerService
{
    /// <summary>
    /// Crawl required number of samples for each language in the <param name="codeFolder"></param>
    /// </summary>
    /// <param name="codeFolder"></param>
    /// <param name="sampleCount"></param>
    /// <returns></returns>
    Task CrawlAsync(string codeFolder, int sampleCount);
}