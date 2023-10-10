using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgLang.CodeCrawler.Models;
using TgLang.CodeCrawler.Services.Contracts;

namespace TgLang.CodeCrawler.Test
{
    public class GitHubServiceTests
    {
        [Fact]
        public async Task GetLanguageRepos_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var gitHubService = testHost.Services.GetRequiredService<IGitHubService>();
            var languageDefService = testHost.Services.GetRequiredService<ILanguageDefService>();

            var csLang = languageDefService.GetLanguageDefs().First(l => l.Extension == "cs");

            var repos = await gitHubService.GetLanguageRepoUrlsAsync(csLang);

            Assert.True(repos.Any());
        }

        [Fact]
        public async Task GetCodeFilesAsync_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var gitHubService = testHost.Services.GetRequiredService<IGitHubService>();

            var files = await gitHubService.GetFilesByUrlAsync("https://github.com/tonradar/tonrich/tree/main/src");

            Assert.True(files.Any());
        }

        [Fact]
        public async Task GetCodeFileContentAsync_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var gitHubService = testHost.Services.GetRequiredService<IGitHubService>();

            var content = await gitHubService.GetCodeFileContentAsync(652218002, "19058653180fa058af1ccf42c2ada0aad7b23144");

            Assert.False(string.IsNullOrWhiteSpace(content));
        }

        [Fact]
        public async Task GetLanguageOfUrl_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var languageDefService = testHost.Services.GetRequiredService<ILanguageDefService>();

            var (language, extension) = languageDefService.GetLanguageOfUrl("https://github.com/tonradar/contest-tglang/blob/main/src/TgLang.CodeCrawler/Services/Contracts/IGitHubService.cs");

            Assert.Equal("cs", language?.Extension);
            Assert.Equal("cs", extension);
        }

        [Fact]
        public async Task SearchFilesAsync_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var gitHubService = testHost.Services.GetRequiredService<IGitHubService>();

            var languageDefService = testHost.Services.GetRequiredService<ILanguageDefService>();
            var csLang = languageDefService.GetLanguageDefs().First(l => l.Extension == "cs");

            var files = await gitHubService.GetFilesBySearchAsync(csLang, 1, 50);

            Assert.Equal(50, files.Count);
        }
    }
}