using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;

namespace TgLang.CodeCrawler.Services.Implementations
{
    public class GitHubService : IGitHubService

    {
        private IHttpClientFactory HttpClientFactory { get; set; }

        public GitHubService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<List<string>> GetLanguageRepoUrlsAsync(LanguageDef languageDef)
        {
            var client = HttpClientFactory.CreateClient();

            var content = await client.GetStringAsync($"https://github.com/topics/{languageDef.GitHubTag}");

            var parser = new HtmlAgilityPack.HtmlDocument();
            parser.LoadHtml(content);
            var html = parser.DocumentNode;

            var repoTags = html.CssSelect("a.Link.text-bold.wb-break-word").ToList();

            var repos = repoTags
                            .Select(r => r.Attributes.FirstOrDefault(a => a.Name == "href")?.Value)
                            .Where(r=>!string.IsNullOrWhiteSpace(r))
                            .Select(r=>$"https://github.com{r}")
                            .ToList();

            return repos;
        }
    }
}
