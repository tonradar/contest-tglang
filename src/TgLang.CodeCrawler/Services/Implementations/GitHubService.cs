using Octokit;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TgLang.CodeCrawler.Exceptions;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;
using Range = Octokit.Range;

namespace TgLang.CodeCrawler.Services.Implementations
{
    public class GitHubService : IGitHubService

    {
        private IHttpClientFactory HttpClientFactory { get; set; }
        public GitHubClient GitHubClient { get; set; }

        public GitHubService(IHttpClientFactory httpClientFactory, GitHubClient gitHubClient)
        {
            HttpClientFactory = httpClientFactory;
            GitHubClient = gitHubClient;
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

        public async Task<List<GitHubFile>> GetCodeFilesAsync(string folderUrl)
        {
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(folderUrl);
            var repo = await GitHubClient.Repository.Get(orgName, repoName);
            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);

            var defaultBranch = repo.DefaultBranch;
            var root = refs.FirstOrDefault(r => r.Ref == $"refs/heads/{defaultBranch}")?.Object.Sha;
            var repositoryId = repo.Id;



            //var folderContents = await GitHubClient.Repository.Content.GetAllContents(repositoryId);
            //var folderSha = "";// folderContents?.First(f => f.Name == lastSegment).Sha;
            var allContents = await GitHubClient.Git.Tree.GetRecursive(repositoryId, root);

            var files = allContents.Tree
                              .Where(t => t.Type == TreeType.Blob)
                              .Select(t => new GitHubFile
                              {
                                  Path = t.Path,
                                  Size = t.Size,
                                  RepoId = repositoryId, 
                                  Sha = t.Sha, 
                                  Url = t.Url
                              })
                              .ToList();

            return files;
        }

        public async Task<string> GetCodeFileContentAsync(long repositoryId, string sha)
        {
            var blob = await GitHubClient.Git.Blob.Get(repositoryId, sha);
            var bytes = Convert.FromBase64String(blob.Content);
            var content = Encoding.UTF8.GetString(bytes);
            return content;

        }

        /// <summary>
        /// Retrieves a repository and organization/owner name from GitHub URL.
        /// </summary>
        /// <param name="url">A GitHub URL</param>
        /// <returns>The repository and organization/owner name.</returns>
        private (string org, string repo) GetRepoAndOrgNameFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var org = segments[1].TrimEnd('/');
            var repo = segments[2].TrimEnd('/');

            return (org, repo);
        }

        public async Task<List<GitHubFile>> SearchFilesAsync(LanguageDef language, int pageNo, int pageSize)
        {
            SearchCodeRequest searchRequest;

            if (
                language.GitHubLanguage is not null 
                ||
                !string.IsNullOrWhiteSpace(language.Extension))
            {
                searchRequest = new SearchCodeRequest()
                {
                    Language = language.GitHubLanguage,
                    Extensions = string.IsNullOrWhiteSpace(language.Extension)
                        ? new string[] { }
                        : new[] { language.Extension },
                    Size = Range.GreaterThan(2000),
                    Page = pageNo,
                    PerPage = pageSize
                };
            }

            //if (language.GitHubLanguage is not null)
            //{
                
            //}
            //else if (!string.IsNullOrWhiteSpace(language.Extension))
            //{
            //    searchRequest = new SearchCodeRequest()
            //    {
            //        //Language = language.GitHubLanguage,
            //        Extensions = new[] { language.Extension },
            //        Size = Range.GreaterThan(2000),
            //        Page = pageNo,
            //        PerPage = pageSize
            //    };
            //}
            else
            {
                throw new UnsupportedLanguageException();
            }

            try
            {
                var result = await GitHubClient.Search.SearchCode(searchRequest);


                var files =
                    from item in result.Items
                    select new GitHubFile()
                    {
                        Path = item.Path,
                        Sha = item.Sha,
                        Url = item.Url,
                        RepoId = item.Repository.Id,
                        Name = item.Name,
                    };

                return files.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var limit = await GitHubClient.RateLimit.GetRateLimits();

                var duration = (limit.Resources.Search.Reset) - DateTimeOffset.Now;

                var result = await GitHubClient.Search.SearchCode(searchRequest);

                throw;
            }
        }
    }
}
