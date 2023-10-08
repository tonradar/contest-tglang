using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    }
}